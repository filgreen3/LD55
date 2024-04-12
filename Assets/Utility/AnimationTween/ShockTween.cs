using UnityEngine;

namespace Simple.Tween
{
    public abstract class ShockTween<T> : Tween<T>
    {
        [SerializeField] protected float _power;

        public void ShockTo(float power, float time)
        {
            _power = power;
            AnimateTo(default, 1f / time);
        }
    }
}
