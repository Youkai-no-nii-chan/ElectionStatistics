using System.Linq;

namespace ElectionInfo.Model
{
    public class ElectoralDistrictsRepository : Repository<ElectoralDistrict>
    {
        public ElectoralDistrictsRepository(ModelContext context) : base(context)
        {
        }

        public ElectoralDistrict GetOrCreate(string name, ElectoralDistrict higherDistrict)
        {
            ElectoralDistrict district;
            if (higherDistrict == null)
            {
                district = Context.ElectoralDistricts.SingleOrDefault(d =>
                    d.Name == name &&
                    d.HigherDistrictId == null);
            }
            else
            {
                district = Context.ElectoralDistricts.SingleOrDefault(d =>
                    d.Name == name &&
                    d.HigherDistrictId == higherDistrict.Id);
            }

            return district ?? Create(name, higherDistrict);
        }

        public ElectoralDistrict Create(string name, ElectoralDistrict higherDistrict)
        {
            var district = new ElectoralDistrict
            {
                Name = name,
                HigherDistrict = higherDistrict,
                HierarchyPath = higherDistrict == null ? null : higherDistrict.HierarchyPath + "\\" + higherDistrict.Id
            };

            Context.ElectoralDistricts.Add(district);
            Context.SaveChanges();

            return district;
        }

        public ElectoralDistrict GetOrCreateByUniqueName(string name)
        {
            var district = Context.ElectoralDistricts.SingleOrDefault(d => d.Name == name);

            if (district == null)
            {
                district = new ElectoralDistrict
                {
                    Name = name,
                    HigherDistrict = null,
                    HierarchyPath = null
                };

                Context.ElectoralDistricts.Add(district);
                Context.SaveChanges();
            }

            return district;
        }
    }
}