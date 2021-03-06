namespace Server.Items
{
    public class BluePlainRugAddon : BaseAddon
    {
        [Constructible]
        public BluePlainRugAddon()
        {
            AddComponent(new LocalizedAddonComponent(0xAC2, 1076585), 1, 1, 0);
            AddComponent(new LocalizedAddonComponent(0xAC3, 1076585), -1, -1, 0);
            AddComponent(new LocalizedAddonComponent(0xAC4, 1076585), -1, 1, 0);
            AddComponent(new LocalizedAddonComponent(0xAC5, 1076585), 1, -1, 0);
            AddComponent(new LocalizedAddonComponent(0xAF6, 1076585), -1, 0, 0);
            AddComponent(new LocalizedAddonComponent(0xAF7, 1076585), 0, -1, 0);
            AddComponent(new LocalizedAddonComponent(0xAF8, 1076585), 1, 0, 0);
            AddComponent(new LocalizedAddonComponent(0xAF9, 1076585), 0, 1, 0);
            AddComponent(new LocalizedAddonComponent(0xAC0, 1076585), 0, 0, 0);
        }

        public BluePlainRugAddon(Serial serial) : base(serial)
        {
        }

        public override BaseAddonDeed Deed => new BluePlainRugDeed();

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadEncodedInt();
        }
    }

    public class BluePlainRugDeed : BaseAddonDeed
    {
        [Constructible]
        public BluePlainRugDeed() => LootType = LootType.Blessed;

        public BluePlainRugDeed(Serial serial) : base(serial)
        {
        }

        public override BaseAddon Addon => new BluePlainRugAddon();
        public override int LabelNumber => 1076585; // Blue plain rug

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadEncodedInt();
        }
    }
}
