using CreativeSpore.SmartColliders;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    public enum ePlayerState
    {
        Stand,
        Walk,
        Jump,
        Fall,
        Climb,
        ClimbStand,
        Crouch,
    }

    public delegate void OnStateChangedDelegate(PlayerAnimation source, ePlayerState prevState, ePlayerState newState);

    /// <summary>
    /// Called when State changed to a different state
    /// </summary>
    public OnStateChangedDelegate OnStateChanged;

    /// <summary>
    /// This is used to flip the sprite properly when facing each direction.
    /// If true, it means, the sprite will be flipped when facing left
    /// </summary>
    public bool IsSpriteFacingRight = true;

    private ePlayerState m_state = ePlayerState.Stand;
    private ePlayerState m_nextState = ePlayerState.Stand;

    private PlayerController m_platformCtrl;
    private Animator m_animator;
    void Start()
    {
        m_platformCtrl = GetComponent<PlayerController>();
        m_animator = GetComponent<Animator>();
        OnStateChanged += _OnStateChanged;
    }

    private void _OnStateChanged(PlayerAnimation source, ePlayerState prevState, ePlayerState newState)
    {
        if (m_animator != null)
        {
            //m_animator.ResetTrigger(prevState.ToString()); //NOTE: be sure the last trigger is the one used in the animator
            m_animator.Play(newState.ToString());
        }
    }

    void Update()
    {
        if (m_state != m_nextState)
        {
            ePlayerState prevState = m_state;
            m_state = m_nextState;
            if (OnStateChanged != null)
            {
                OnStateChanged(this, prevState, m_state);
            }
        }

        // Flip player when facing opposite direction
        float absScaleX = Mathf.Abs(transform.localScale.x);
        if (m_platformCtrl.GetActionState(eControllerActions.Left))
        {
            transform.localScale = new Vector3(IsSpriteFacingRight ? -absScaleX : absScaleX, transform.localScale.y, transform.localScale.z);
        }
        else if (m_platformCtrl.GetActionState(eControllerActions.Right))
        {
            transform.localScale = new Vector3(IsSpriteFacingRight ? absScaleX : -absScaleX, transform.localScale.y, transform.localScale.z);
        }

        if (m_platformCtrl.IsClimbing)
        {
            m_nextState = m_platformCtrl.GetActionState(
                eControllerActions.Left | eControllerActions.Right |
                eControllerActions.Up | eControllerActions.Down) ? ePlayerState.Climb : ePlayerState.ClimbStand;
        }
        else if (m_platformCtrl.IsGrounded)
        {
            if(m_platformCtrl.GetActionState(eControllerActions.Down))
            {
                m_nextState =  ePlayerState.Crouch;
            }
            else
                m_nextState = m_platformCtrl.GetActionState(eControllerActions.Left | eControllerActions.Right) ? ePlayerState.Walk : ePlayerState.Stand;
        }
        else
        {
            m_nextState = m_platformCtrl.PlatformCharacterPhysics.VSpeed > 0f ? ePlayerState.Jump : ePlayerState.Fall;
        }
    }
}