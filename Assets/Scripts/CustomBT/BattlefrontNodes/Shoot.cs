namespace BT.CustomLeaves
{
    public class Shoot : AILeaf
    {
        public override object Run()
        {
            return Gd.Get<Gun>("Gun").Shoot() ?
                1 :
                0;
        }
    }
}