
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;
using System.Collections.Generic;
using System.Linq;
using SWB_Base;

public class InventoryColumn : Panel
{
    public int Column;
    public bool IsSelected;
    public Label Header;
    public int SelectedIndex;

    internal List<InventoryIconDM> Icons = new();

    public InventoryColumn( int i, Panel parent )
    {
        Parent = parent;
        Column = i;
        Header = Add.Label( $"{i + 1}", "slot-number" );
    }

    internal void UpdateWeapon( WeaponBase weapon )
    {
        var icon = ChildrenOfType<InventoryIconDM>().FirstOrDefault( x => x.Weapon == weapon );
        if ( icon == null )
        {
            icon = new InventoryIconDM( weapon );
            icon.Parent = this;
            Icons.Add( icon );
        }
    }

    internal void TickSelection( WeaponBase selectedWeapon )
    {
        SetClass( "active", selectedWeapon?.Bucket == Column );

        for ( int i = 0; i < Icons.Count; i++ )
        {
            Icons[i].TickSelection( selectedWeapon );
        }

    }
}
