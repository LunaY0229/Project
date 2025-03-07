using Assets.Scripts.HumanControl;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamePlay.HumanFSM
{
    public class HumanFSM : BaseCharactorFSM<HumanState>
    {
        public Transform targetl;
        public HumanData humanData;
        public GameObject b;
        public GameObject b_1;

        public float maxHealth = 2000f;
        public float currentHealth = 2000f;

        public GameObject winPanel;

        private void Start()
        {
            humanData.runTimeData.currentPower = 100f;
            humanData.runTimeData.target = targetl;
            states.Add(HumanState.Normal, new NormalStates(this));
            states.Add(HumanState.Hit, new HitState(this));
            states.Add(HumanState.Fly, new HumanFlyLocamotion(this));
            ChangState(HumanState.Normal);

            currentHealth = maxHealth;
        }


        protected override void Update()
        {
            base.Update();
            states[currentState].OnUpdate();

            if (humanData.runTimeData.currentPower < humanData.normalData.maxPowerNum)
                humanData.runTimeData.currentPower += Time.deltaTime * humanData.normalData.powerGetSpeed;
        }

        public override void Demage(Transform attacker, float hit, float force)
        {
            if (humanData.runTimeData.isInvincible)
            {
                var obj = GameObjectPool.Instance.GetGameObjectByName("Bo");
                obj.transform.position = transform.position + (attacker.position - transform.position).normalized + Vector3.up;
                obj.transform.forward = transform.forward;

                CountTime(2, () =>
                {
                    GameObjectPool.Instance.PushObj(obj);
                });

                return;
            }
            else
            {
                currentHealth -= hit;

                if(currentHealth <= 0)
                {
                    winPanel.SetActive(true);
                }

                if (humanData.runTimeData.canHitBack)
                {
                    //(states[HumanState.Hit] as HitState).SetAttacker(attacker, force);
                    //ChangState(HumanState.Hit);
                }
                else
                {
                    humanData.runTimeData.currentHealth -= hit;
                }
            }
        }

        public void AttackCheck(string f)
        {
            string[] data = f.Split("_");
            var combo = int.Parse(data[0]) - 1;
            var index = int.Parse(data[1]) - 1;

            Debug.Log("Combo" + (combo + 1) + "_" + (index));

            transform.Find("Combo" + (combo + 1) + "_" + (index)).gameObject.SetActive(true);

            CountTime(2f,() =>
            {
                transform.Find("Combo" + (combo + 1) + "_" + (index)).gameObject.SetActive(false);
            });

            if (humanData.runTimeData.normalAttackList[combo].actionTimeClip.Count > index)
            {
                SourcesManager.Instance.PlayOnShot(humanData.runTimeData.normalAttackList[combo].actionTimeClip[index], 1f);
            }
            var force = humanData.runTimeData.normalAttackList[combo].force[index];

            CameraShake(0.25f);
            DemageEnemy(1,1,0,1.3f,10, force);
        }

        public void RangedAttack(string f)
        {
            string[] data = f.Split("_");
            //var combo = int.Parse(data[0]) - 1;
            //var index = int.Parse(data[1]) - 1; 

            var obj = GameObjectPool.Instance.GetGameObjectByName("JQ");
            obj.transform.position = transform.position + transform.forward;
            obj.transform.forward = transform.forward;

            //StartCoroutine(CheckParticleCollision(obj.GetComponentInChildren<ParticleSystem>()));

            CountTime(2,() =>
            {
                GameObjectPool.Instance.PushObj(obj);
            });
            CameraShake(1f);
        }

        // 协程持续检查粒子碰撞
        private IEnumerator CheckParticleCollision(ParticleSystem particleSystem)
        {
            while (true)
            {
                // 获取当前粒子的位置信息（包括速度）
                ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particleSystem.main.maxParticles];
                int numParticlesAlive = particleSystem.GetParticles(particles);

                if (numParticlesAlive > 0)
                {
                    foreach (var particle in particles)
                    {
                        Vector3 particlePosition = particle.position;
                        Vector3 particleVelocity = particle.velocity;

                        // 预测粒子未来位置
                        Vector3 predictedPosition = particlePosition + particleVelocity * Time.deltaTime;

                        // 适当扩大检测范围来考虑粒子的速度
                        bool enemyDamaged = DemageEnemy(predictedPosition, 5f, 10.0f, 5.0f); // 假设攻击范围是3.0f，伤害是10.0f，击退力是5.0f



                        if (enemyDamaged)
                        {
                            Debug.Log("Enemy damaged by particle at position " + predictedPosition);
                            yield break; // 如果有敌人受伤，退出协程
                        }
                    }
                }

                // 等待下一帧再检查
                yield return null;
            }
        }


        public void CameraShake(float shakeForce)
        {
            if (shakeForce == 0) { return; }

            var cinemachineImpulseSource = GameObject.Find("CameraShark").GetComponent<CinemachineImpulseSource>();
            cinemachineImpulseSource.GenerateImpulseWithForce(shakeForce);
        }


        public float GetDistance()
        {
            return Vector3.Distance(transform.position, humanData.runTimeData.target.position);
        }

        public bool IsAnyRangedAttackCanDo()
        {
            for (int i = 0; i < humanData.runTimeData.rangedAttackList.Count; i++)
            {
                if (!humanData.runTimeData.rangedAttackList[i].isCD)
                {
                    if (humanData.runTimeData.rangedAttackList[i].costPower < humanData.runTimeData.currentPower)
                        return true;
                }
            }

            return false;
        }

        public bool IsAnyNormalAttackCanDo()
        {
            for (int i = 0; i < humanData.runTimeData.normalAttackList.Count; i++)
            {
                if (!humanData.runTimeData.normalAttackList[i].isCD)
                {
                    if (humanData.runTimeData.normalAttackList[i].costPower < humanData.runTimeData.currentPower)
                        return true;
                }
            }

            return false;
        }

        public bool DemageEnemy(Vector3 pos,float radius, float DeamgeNum, float DemageForce)
        {
            Collider[] enemiesInRange = Physics.OverlapSphere(pos, radius);

            foreach (var enemy in enemiesInRange)
            {
                // 判断敌人是否符合攻击条件
                if (enemy.CompareTag("Player"))
                {
                    if (enemy.gameObject != gameObject)
                    {
                        enemy.GetComponent<FSM>().Demage(transform, DeamgeNum, DemageForce);
                        return true;
                    }
                }
            }
            return false;
        }

        public bool DemageEnemy(float SkillCheckForwardOffset, float SkillCheckUpOffset, float SkillCheckRightOffset, float SkillCheckRadius, float DeamgeNum, float DemageForce)
        {
            var pos = transform.position + SkillCheckForwardOffset * transform.forward + SkillCheckUpOffset * Vector3.up + SkillCheckRightOffset * transform.right;

            var Obj = new GameObject("Check_1");
            Obj.transform.position = pos;

            var radius = SkillCheckRadius;

            // 使用 OverlapSphere 来检测攻击范围内的敌人
            Collider[] enemiesInRange = Physics.OverlapSphere(pos, radius);

            foreach (var enemy in enemiesInRange)
            {
                // 判断敌人是否符合攻击条件
                if (enemy.CompareTag("Player"))
                {
                    if (enemy.gameObject != gameObject)
                    {
                        enemy.GetComponent<FSM>().Demage(transform, DeamgeNum, DemageForce);
                        return true;
                    }
                }
            }
            return false;
        }

        private void OnDrawGizmos()
        {
            var pos = transform.position + 1.3f * transform.forward + 1 * transform.up;
            Gizmos.DrawSphere(pos,1);
        }

        public void ChangeAlpha_1()
        {
            b_1.SetActive(true);
            StartCoroutine(ChangeAlpha(b.GetComponent<Renderer>().material));
            StartCoroutine(ChangeAlpha(b_1.GetComponent<Renderer>().material));
        }

        IEnumerator ChangeAlpha(Material mesh)
        {
            float value = mesh.GetFloat("_Alpha");
            while (value < 1)
            {
                value += Time.deltaTime;
                mesh.SetFloat("_Alpha", value);
                yield return null;
            }
        }
    }

    

    public enum HumanState
    {
        None,
        Normal,
        Hit,
        Fly
    }
}
