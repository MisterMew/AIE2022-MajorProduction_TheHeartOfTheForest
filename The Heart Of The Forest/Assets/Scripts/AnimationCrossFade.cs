/*
 * Date Created: 04/11/2022
 * Author: Nicholas Connell
 */

using UnityEngine;

public class AnimationCrossFade : MonoBehaviour
{
    [SerializeField] private Animator m_animator;

    /// <summary>
    /// Fades to an animation
    /// </summary>
    /// <param name="anim">The name of the animation</param>
    public void FadeToAnim(string anim)
    {
        m_animator.CrossFade(anim, 0, -1, 0);
    }
}
