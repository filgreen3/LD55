using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int Damage;
    public Rigidbody2D Rig2D;
    public SpriteRenderer _spriteRenderer;

    public void SetColor(Color color) => _spriteRenderer.color = color;

    public void SetDamage(int damage) => Damage = damage;

    public void SetLayer(ProjectileType projectileType) => gameObject.layer = projectileType == ProjectileType.ToCharacter ? 10 : 11;

    public void Launch(Vector2 direction, float force)
    {
        Rig2D.AddForce(direction * force, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent(out Character enemy))
        {
            enemy.GetHealth().HealthPoints -= Damage;
        }
        else if (other.gameObject.TryGetComponent(out Organ organ))
        {
            organ.GetHealth().Value -= Damage;
        }
        ProjectileHelperSystem.ReleaseProjectile(this);
    }
}

public enum ProjectileType
{
    ToCharacter,
    ToOrgan
}