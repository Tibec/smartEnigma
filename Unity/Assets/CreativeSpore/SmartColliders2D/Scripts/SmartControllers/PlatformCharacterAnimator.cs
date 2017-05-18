using UnityEngine;
using System.Collections;

namespace CreativeSpore.SmartColliders
{
    [RequireComponent(typeof(PlatformCharacterController))]
    [RequireComponent(typeof(Animator))]
    public class PlatformCharacterAnimator : MonoBehaviour
    {
        public enum eState
        {
            Stand,
            Walk,
            Jump,
            Fall,
            Climb,
            ClimbStand,
            Dying,
        }

        public delegate void OnStateChangedDelegate(PlatformCharacterAnimator source, eState prevState, eState newState);

        /// <summary>
        /// Called when State changed to a different state
        /// </summary>
        public OnStateChangedDelegate OnStateChanged;

        /// <summary>
        /// This is used to flip the sprite properly when facing each direction.
        /// If true, it means, the sprite will be flipped when facing left
        /// </summary>
        public bool IsSpriteFacingRight = true;

        private eState m_state = eState.Stand;
        private eState m_nextState = eState.Stand;

        private PlatformCharacterController m_platformCtrl;
        private Animator m_animator;
        void Start()
        {
            m_platformCtrl = GetComponent<PlatformCharacterController>();
            m_animator = GetComponent<Animator>();
            OnStateChanged += _OnStateChanged;
        }

        private void _OnStateChanged(PlatformCharacterAnimator source, eState prevState, eState newState)
        {
            if (m_animator != null)
            {
                m_animator.ResetTrigger(prevState.ToString()); //NOTE: be sure the last trigger is the one used in the animator
                m_animator.Play(newState.ToString());
            }
        }

        void Update()
        {
            if (m_state != m_nextState)
            {
                eState prevState = m_state;
                m_state = m_nextState;
                if (OnStateChanged != null)
                {
                    OnStateChanged(this, prevState, m_state);
                }
            }

            // Flip player when facing opposite direction
            float absScaleX = Mathf.Abs(transform.localScale.x);
            if( m_platformCtrl.GetActionState(eControllerActions.Left) )
            {
                transform.localScale = new Vector3(IsSpriteFacingRight ? -absScaleX : absScaleX, transform.localScale.y, transform.localScale.z);
            }
            else if( m_platformCtrl.GetActionState(eControllerActions.Right) )
            {
                transform.localScale = new Vector3(IsSpriteFacingRight ? absScaleX : -absScaleX, transform.localScale.y, transform.localScale.z);
            }

            if( m_platformCtrl.IsClimbing )
            {
                m_nextState = m_platformCtrl.GetActionState(
                    eControllerActions.Left | eControllerActions.Right | 
                    eControllerActions.Up | eControllerActions.Down) ? eState.Climb : eState.ClimbStand;
            }
            else if(m_platformCtrl.IsGrounded)
            {
                m_nextState = m_platformCtrl.GetActionState(eControllerActions.Left | eControllerActions.Right) ? eState.Walk : eState.Stand;              
            }
            else
            {
                m_nextState = m_platformCtrl.PlatformCharacterPhysics.VSpeed > 0f ? eState.Jump: eState.Fall;
            }
        }


    }
}