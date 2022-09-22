using UnityEngine;
using UnityEngine.UI;

public class BulletBar : ProgressBar 
{
    [SerializeField] private RawImage m_progressBorderTile;
    [SerializeField] private PlayerWeaponController m_weaponController;

    protected override void Update() 
    {
        base.Update();
        
        m_progressBorderTile.uvRect = new Rect(m_progressBorderTile.uvRect.x, m_progressBorderTile.uvRect.y, m_weaponController.magazineSize, m_progressBorderTile.uvRect.height);
        SetProgress(m_weaponController.bulletsLeft, m_weaponController.magazineSize);
    }
}