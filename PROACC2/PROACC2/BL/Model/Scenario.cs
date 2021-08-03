using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PROACC2.BL.Model
{
    public class Scenario
    {
        public int ScenarioId { get; set; }

        public String ScenarioName { get; set; }

        public String BuildingBlock_Id { get; set; }

        public bool isActive { get; set; }

		public bool isDelete { get; set; }

        public Guid Cre_By { get; set; }

        public Guid Modified_by { get; set; }

        public List<int> TagIds { get; set; }

        public bool EditStatus { get; set; }
        

    }
}