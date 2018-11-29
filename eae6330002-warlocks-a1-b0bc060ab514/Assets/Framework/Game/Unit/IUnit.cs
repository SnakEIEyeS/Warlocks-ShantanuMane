public interface IUnit {

    string Name { get; }
    IUnitHealthHandler UnitHealthHandler { get; }
    IHealth Health { get; }
    Player Owner { get; }

}
