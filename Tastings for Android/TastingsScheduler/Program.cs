using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace TastingsScheduler
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                List<AmountsMovements> LstObjWall= new List<AmountsMovements>();
                List<AmountsMovements> LstObjPP = new List<AmountsMovements>();
                AmountsMovements obj = null;
                string connectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("checkAmountsmovement", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;
                        conn.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        if (ds != null && ds.Tables.Count > 0)
                        {
                             if(ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                            {
                                DataTable dt = ds.Tables[0];                                
                                foreach(DataRow dr in dt.Rows)
                                {
                                    obj = new AmountsMovements();
                                    //obj.WineId = Convert.ToInt32(dr["WineId"]);
                                    obj.LabelName = dr["LabelName"].ToString();
                                    //obj.BarCode = dr["Barcode"].ToString();
                                    obj.CustomerId = dr["CustomerId"].ToString();
                                    obj.Token = dr["DeviceToken"].ToString();
                                    LstObjWall.Add(obj);
                                }
                            }
                            if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                            {
                                DataTable dt = ds.Tables[1];
                                foreach (DataRow dr in dt.Rows)
                                {
                                    obj = new AmountsMovements();
                                    //obj.WineId = Convert.ToInt32(dr["WineId"]);
                                    obj.LabelName = dr["LabelName"].ToString();
                                   //obj.BarCode = dr["Barcode"].ToString();
                                    obj.CustomerId = dr["CustomerId"].ToString();
                                    obj.Token = dr["DeviceToken"].ToString();
                                    LstObjPP.Add(obj);
                                }
                            }

                            MessageSender ms = new MessageSender();
                            for (int i = 0; i < LstObjWall.Count; i++)
                            {

                                ms.SendNotification(LstObjWall[i].Token.Replace(',', ':'),LstObjWall[i].LabelName);
                            }
                            for (int i = 0; i < LstObjPP.Count; i++)
                            {

                                ms.SendNotification(LstObjPP[i].Token.Replace(',', ':'),LstObjPP[i].LabelName);
                            }
                        }
                    }
                }
            }   
            catch(Exception ex)
            {

            }
        }
    }
}
