using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractibleInMenu : Interactible
{
    public override bool LockMove { get { return true; } }
    protected List<Menu> menus;
    public new abstract void UnInteract(GameObject player);

    protected void RefreshMenus()
    {
        foreach (Menu menu in menus)
        {
            menu.Refresh();
        }
    }

    protected void DeleteUi(GameObject player)
    {
        for (int i = menus.Count - 1; i >= 0; i--)
        {
            Menu menu = menus[i];
            if (menu.BelongsToPlayer(player))
            {
                menus.Remove(menu);
                Destroy(menu.gameObject);
            }
        }
    }

}
