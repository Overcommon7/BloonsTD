using UnityEngine;

public static class SuperMonkey
{
    const float RangeIncrease = 10f;
    const float AttackSpeed = 0.085f;
    public const float Range = 6.5f;
    public const int Price = 4000;
    

    static public Upgrade[] Upgrades = new Upgrade[2] { new Upgrade("Epic Range", 2400), new Upgrade("Laser Vision", 3500) };

    public static void Awake(ref TowerVariables values)
    {
        values.range = Range;
        values.sellPrice = (int)(Price * 0.8f);
    }
    public static void Update(ref TowerVariables values, Transform transform)
    {
        values.timeSinceLastShot += Time.deltaTime;
        if (AttackSpeed > values.timeSinceLastShot) return;

        var target = Tower.GetTargetBloon(ref values, transform);
        if (target == null) return;

        transform.right = target.transform.position - transform.position;
        var projectile = Object.Instantiate(values.projectile, transform);
        projectile.gameObject.SetActive(true);
        projectile.transform.right = transform.right;
        values.timeSinceLastShot = 0;
    }
    public static void ApplyUpgrade(int upgradeIdx, ref TowerVariables values)
    {
        if (!Upgrade.PreUpgrade(upgradeIdx)) return;
        if (upgradeIdx == 1)
        {
            var owner = values.projectile.owner;
            values.projectile = System.Array.Find(owner.GetComponentsInChildren<Projectile>(true), x => x.name.Contains("Laser"));
            values.projectile.owner = owner;
            values.projectile.name = "Laser";
            values.projectile.transform.SetParent(values.projectile.owner.transform, false);
            values.projectile.transform.localPosition = new Vector3(0, 0, 0);
            values.sellPrice = (int)((Price + Upgrades[upgradeIdx].price + (values.upgraded[1] ? Upgrades[1].price : 0)) * 0.8f);
        }
        else
        {
            values.range = RangeIncrease;
            values.sellPrice = (int)((Price + Upgrades[upgradeIdx].price + (values.upgraded[0] ? Upgrades[0].price : 0)) * 0.8f);
        }
        SoundManager.Instance.PlaySound(Sounds.LevelUp);
    }
}
