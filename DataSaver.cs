using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static Calculation_Vacation.Page_SWD;

namespace Calculation_Vacation
{
    internal class DataSaver
    {
        private readonly string PATH = Environment.CurrentDirectory+"\\data.json";

        public bool TableIsCreate
        {
            get
            {
                bool FailIsCreat = File.Exists( PATH );
                return FailIsCreat;
            }
        }

        public BindingList<Month_Work> LoadDate()
        {
            var FileDate = File.Exists( PATH );
            if ( !FileDate )
            {
                File.CreateText( PATH ).Dispose();
                return [];
            }
            using (var reader = File.OpenText( PATH ))
            {
                var faletxt = reader.ReadToEnd();

                return JsonConvert.DeserializeObject<BindingList<Month_Work>>( faletxt );
            }
        }

        public void SaveDate( BindingList<Month_Work> month_Works)
        {
            using (StreamWriter writer = File.CreateText( PATH ) )
            {
                string output = JsonConvert.SerializeObject( month_Works );
                writer.WriteLine( output );
            }
        }

        public void DeletData()
        {
            var FileData = File.Exists( PATH );
            if (FileData)
            {
                File.Delete( PATH );
            }
        }
    }
}
