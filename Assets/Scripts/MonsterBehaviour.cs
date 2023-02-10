using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Retro.ThirdPersonCharacter
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Animator))]
    public class MonsterBehaviour : MonoBehaviour
    {
        const float locomotionSmootheningTime = .1f;
        private Animator _animator;
        private CharacterController _characterController;
        Transform _victim;
        NavMeshAgent agent;

        private Vector2 lastMovementInput;
        private Vector3 moveDirection = Vector3.zero;

        public float lookRadius = 15.0f;
        public float gravity = 10;

        public float MaxSpeed = 1;
        public float AttackSpeed = 1f;
        private float DecelerationOnStop = 0.00f;
        private float AttackCooldown = 0f;

    // Start is called before the first frame update
        void Start()
        {
            _animator = GetComponentInChildren<Animator>();
            agent = GetComponent<NavMeshAgent>();
            _victim = PlayerManager.instance.player.transform;
            // _combat = GetComponent<Combat>();
            _characterController = GetComponent<CharacterController>();
        }

    // Update is called once per frame
        void Update()
        {
            if (_animator == null) return;

            AttackCooldown -= Time.deltaTime;

            float distance = Vector3.Distance(_victim.position, transform.position);

            _animator.SetFloat("Motion", agent.velocity.magnitude);

            if( distance <= lookRadius && _animator.GetBool("IsDead") == false)
            {
                agent.SetDestination(_victim.position);
                _animator.SetBool("Pursue", true);

                if(distance <= agent.stoppingDistance)
                {
                    // Attack
                    if ( AttackCooldown <= 0f){
                        _animator.SetTrigger("Attack1");
                        AttackCooldown = 1f / AttackSpeed;
                        GetComponent<Monster>().Attack();
                    }
                    FaceTarget();
                }
            } else {
                _animator.SetBool("Pursue", false);
            }

            float speedPercent = agent.velocity.magnitude / agent.speed;
            _animator.SetFloat("Blend", speedPercent, locomotionSmootheningTime, Time.deltaTime);
            // if(_combat.AttackInProgress)
            // {
            //     StopMovementOnAttack();
            // }
            // else
            // {
            //     Move();
            // }

            if(_animator.GetBool("IsDead") == true){

                StartCoroutine(DelayDestroy());
            }
        }

        void FaceTarget()
        {
            Vector3 direction = (_victim.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 1f);
        }

        private void Move()
        {
            var x = _victim.position.x;
            var y = _victim.position.y;
            var step =  MaxSpeed * Time.deltaTime;
        // var step =  speed * Time.deltaTime; // calculate distance to move
        // transform.position = Vector3.MoveTowards(transform.position, target.position, step);
            bool grounded = true;

            if (grounded)
            {
                moveDirection = new Vector3(x, 0, y);
                moveDirection = Vector3.MoveTowards(transform.position, _victim.position, MaxSpeed);
                moveDirection *= MaxSpeed;
            // if (_playerInput.JumpInput)
                // moveDirection.y = jumpSpeed;
            }

            // moveDirection.y -= gravity * Time.deltaTime;
            _characterController.Move(moveDirection * Time.deltaTime);

            _animator.SetFloat("InputX", x);
            _animator.SetFloat("InputY", y);
            // _animator.SetBool("IsInAir", !grounded);
        }

        private void StopMovementOnAttack()
        {
            var temp = lastMovementInput;
            temp.x -= DecelerationOnStop;
            temp.y -= DecelerationOnStop;
            lastMovementInput = temp;

            _animator.SetFloat("InputX", lastMovementInput.x);
            _animator.SetFloat("InputY", lastMovementInput.y);
        }

        void OnDrawGizmosSelected() {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, lookRadius);
        }

        IEnumerator DelayDestroy(){
            agent.enabled = false;
            yield return new WaitForSeconds(10.0f);
            Destroy(gameObject);
        }
    }
}
