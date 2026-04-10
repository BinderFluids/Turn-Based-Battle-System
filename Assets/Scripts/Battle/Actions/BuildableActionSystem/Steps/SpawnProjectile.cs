using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;


public class SpawnProjectile : BuildableBattleActionStep
{
    [SerializeField] private float m_speed;
    [SerializeField] private float m_duration;
    [SerializeField] private GameObject m_projectilePrefab;
    private List<GameObject> m_SpawnedProjectiles = new List<GameObject>();
    private SocketReference socketReference;
    
    

    public override async UniTask Execute(BattleEntity actor, BattleEntity target)
    {
        //TODO: IMPLEMENT SOME SORT OF PROJECTILE SINGLETON
        
        // if (m_projectilePrefab == null || target == null) return; 
        //
        // GameObject p = Instantiate(m_projectilePrefab, actor.transform.position, Quaternion.identity);
        // m_SpawnedProjectiles.Add(p);
        // Vector3 centerPos = ParentAction.SocketData.GetSocketPositionsDictionary()[m_socket.selectedSocketName]; 
        // centerPos.z = p.transform.position.z;
                    
        // ProjectileEntity projectile = p.GetComponent<ProjectileEntity>();
        // if (projectile != null)
        // {
        //     projectile.Setup(centerPos, m_speed, m_duration);
        // }
    }
}