using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Factions;

public class TownStoneGump : FactionGump
{
    private readonly Faction m_Faction;
    private readonly PlayerMobile m_From;
    private readonly Town m_Town;

    public TownStoneGump(PlayerMobile from, Faction faction, Town town) : base(50, 50)
    {
        m_From = from;
        m_Faction = faction;
        m_Town = town;

        AddPage(0);

        AddBackground(0, 0, 320, 250, 5054);
        AddBackground(10, 10, 300, 230, 3000);

        AddHtmlText(25, 30, 250, 25, town.Definition.TownStoneHeader, false, false);

        AddHtmlLocalized(55, 60, 150, 25, 1011557); // Hire Sheriff
        AddButton(20, 60, 4005, 4007, 1);

        AddHtmlLocalized(55, 90, 150, 25, 1011559); // Hire Finance Minister
        AddButton(20, 90, 4005, 4007, 2);

        AddHtmlLocalized(55, 120, 150, 25, 1011558); // Fire Sheriff
        AddButton(20, 120, 4005, 4007, 3);

        AddHtmlLocalized(55, 150, 150, 25, 1011560); // Fire Finance Minister
        AddButton(20, 150, 4005, 4007, 4);

        AddHtmlLocalized(55, 210, 150, 25, 1011441); // EXIT
        AddButton(20, 210, 4005, 4007, 0);
    }

    public override void OnResponse(NetState sender, in RelayInfo info)
    {
        if (m_Town.Owner != m_Faction || !m_Faction.IsCommander(m_From))
        {
            m_From.SendLocalizedMessage(1010339); // You no longer control this city
            return;
        }

        switch (info.ButtonID)
        {
            case 1: // hire sheriff
                {
                    if (m_Town.Sheriff != null)
                    {
                        // You must fire your Sheriff before you can elect a new one
                        m_From.SendLocalizedMessage(1010342);
                    }
                    else
                    {
                        m_From.SendLocalizedMessage(1010347); // Who shall be your new sheriff
                        m_From.BeginTarget(12, false, TargetFlags.None, HireSheriff_OnTarget);
                    }

                    break;
                }
            case 2: // hire finance minister
                {
                    if (m_Town.Finance != null)
                    {
                        // You must fire your finance minister before you can elect a new one
                        m_From.SendLocalizedMessage(1010345);
                    }
                    else
                    {
                        m_From.SendLocalizedMessage(1010348); // Who shall be your new Minister of Finances?
                        m_From.BeginTarget(12, false, TargetFlags.None, HireFinanceMinister_OnTarget);
                    }

                    break;
                }
            case 3: // fire sheriff
                {
                    if (m_Town.Sheriff == null)
                    {
                        // You need to elect a sheriff before you can fire one
                        m_From.SendLocalizedMessage(1010350);
                    }
                    else
                    {
                        m_From.SendLocalizedMessage(1010349);         // You have fired your sheriff
                        m_Town.Sheriff.SendLocalizedMessage(1010270); // You have been fired as Sheriff
                        m_Town.Sheriff = null;
                    }

                    break;
                }
            case 4: // fire finance minister
                {
                    if (m_Town.Finance == null)
                    {
                        // You need to elect a financial minister before you can fire one
                        m_From.SendLocalizedMessage(1010352);
                    }
                    else
                    {
                        m_From.SendLocalizedMessage(1010351);         // You have fired your financial Minister
                        m_Town.Finance.SendLocalizedMessage(1010151); // You have been fired as Finance Minister
                        m_Town.Finance = null;
                    }

                    break;
                }
        }
    }

    private void HireSheriff_OnTarget(Mobile from, object obj)
    {
        if (m_Town.Owner != m_Faction || !m_Faction.IsCommander(from))
        {
            from.SendLocalizedMessage(1010339); // You no longer control this city
            return;
        }

        if (m_Town.Sheriff != null)
        {
            from.SendLocalizedMessage(1010342); // You must fire your Sheriff before you can elect a new one
            return;
        }

        if (obj is not Mobile m)
        {
            from.SendLocalizedMessage(1010334); // You must select a player to hold a city position!
            return;
        }

        var pl = PlayerState.Find(m);

        if (pl == null)
        {
            from.SendLocalizedMessage(1010337); // You must pick someone in a faction
        }
        else if (pl.Faction != m_Faction)
        {
            from.SendLocalizedMessage(1010338); // You must pick someone in the correct faction
        }
        else if (m_Faction.Commander == m)
        {
            from.SendLocalizedMessage(1010335); // You cannot elect a commander to a town position
        }
        else if (pl.Sheriff != null || pl.Finance != null)
        {
            // You must pick someone who does not already hold a city post
            from.SendLocalizedMessage(1005245);
        }
        else
        {
            m_Town.Sheriff = m;
            m.SendLocalizedMessage(1010340); // You are now the Sheriff
            from.SendLocalizedMessage(1010341); // You have elected a Sheriff
        }
    }

    private void HireFinanceMinister_OnTarget(Mobile from, object obj)
    {
        if (m_Town.Owner != m_Faction || !m_Faction.IsCommander(from))
        {
            from.SendLocalizedMessage(1010339); // You no longer control this city
            return;
        }

        if (m_Town.Finance != null)
        {
            from.SendLocalizedMessage(1010342); // You must fire your Sheriff before you can elect a new one
            return;
        }

        if (obj is not Mobile m)
        {
            from.SendLocalizedMessage(1010334); // You must select a player to hold a city position!
            return;
        }

        var pl = PlayerState.Find(m);

        if (pl == null)
        {
            from.SendLocalizedMessage(1010337); // You must pick someone in a faction
        }
        else if (pl.Faction != m_Faction)
        {
            from.SendLocalizedMessage(1010338); // You must pick someone in the correct faction
        }
        else if (m_Faction.Commander == m)
        {
            from.SendLocalizedMessage(1010335); // You cannot elect a commander to a town position
        }
        else if (pl.Sheriff != null || pl.Finance != null)
        {
            // You must pick someone who does not already hold a city post
            from.SendLocalizedMessage(1005245);
        }
        else
        {
            m_Town.Finance = m;
            m.SendLocalizedMessage(1010343); // You are now the Financial Minister
            from.SendLocalizedMessage(1010344); // You have elected a Financial Minister
        }
    }
}
