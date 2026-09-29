using UnityEngine;
using System.Collections.Generic;

public class WeaponSystem : MonoBehaviour
{
    [System.Serializable]
    public class Weapon
    {
        public string name;
        public int damage;
        public float fireRate;
        public int magazineSize;
        public float range;
        public float accuracy;
    }
    
    [SerializeField] private List<Weapon> weapons = new List<Weapon>();
    [SerializeField] private Transform muzzlePoint;
    [SerializeField] private Camera playerCamera;
    
    private int currentWeaponIndex = 0;
    private int currentAmmo;
    private float lastFireTime = 0f;
    private NetworkManager networkManager;
    
    private void Start()
    {
        networkManager = NetworkManager.Instance;
        if (weapons.Count > 0)
        {
            currentAmmo = weapons[0].magazineSize;
        }
    }
    
    public void Fire()
    {
        Weapon weapon = weapons[currentWeaponIndex];
        
        if (Time.time - lastFireTime < 1f / weapon.fireRate || currentAmmo <= 0)
            return;
        
        lastFireTime = Time.time;
        currentAmmo--;
        
        // Raycast for hit detection
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, weapon.range))
        {
            float distance = Vector3.Distance(transform.position, hit.point);
            string hitLocation = DetermineHitLocation(hit.collider.gameObject);
            
            // Get target socket ID from hit player
            PlayerController targetPlayer = hit.collider.GetComponent<PlayerController>();
            if (targetPlayer != null)
            {
                string targetSocketId = targetPlayer.gameObject.name; // Should store actual socket ID
                networkManager.FireWeapon(targetSocketId, weapon.name, distance, hitLocation);
                
                // Visual feedback
                Debug.Log($"Hit {targetSocketId} for {weapon.damage} damage at {hitLocation}");
            }
        }
    }
    
    public void Reload()
    {
        Weapon weapon = weapons[currentWeaponIndex];
        currentAmmo = weapon.magazineSize;
        Debug.Log("Reloaded " + weapon.name);
    }
    
    public void SwitchWeapon()
    {
        currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Count;
        currentAmmo = weapons[currentWeaponIndex].magazineSize;
        Debug.Log("Switched to " + weapons[currentWeaponIndex].name);
    }
    
    private string DetermineHitLocation(GameObject hitObject)
    {
        // Simple hit location detection based on object name or position
        if (hitObject.name.Contains("Head"))
            return "head";
        if (hitObject.name.Contains("Leg"))
            return "legs";
        if (hitObject.name.Contains("Arm"))
            return "arms";
        return "torso";
    }
}
