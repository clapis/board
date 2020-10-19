using System.Collections.Generic;

namespace Board.Application.Companies.GetCompanies
{
    public class GetCompaniesQueryResult
    {
        public List<Company> Companies { get; }

        public GetCompaniesQueryResult(List<Company> companies)
        {
            Companies = companies;
        }

        public class Company
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public bool HasLogo { get; set; }

            public string Website { get; set; }
        }
    }
}
