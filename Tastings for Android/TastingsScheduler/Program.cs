using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using NLog;

namespace TastingsScheduler
{
    public class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static void Main(string[] args)
        {
            //  MessageSenderIOS msIOs = new MessageSenderIOS();
            //msIOs.sendMessage("0001000001189", "d85a40ea517601c397a2e0b404157bb4164134e17a1a822adbf7470f0093d047", "Just Testing notification", 1);
            // msIOs.SendNotification("cFPtPgzgg1o,APA91bFt7Tp1mb1dthhHQVqDkbQHUapp-O1xrk9rKiLdLbJGJltq32smRjNE1lihbnN7sf5j8Fnjii2-o_MlDap-GB-IyqXBtZkJc4h0XxZzWFGRtOmyT9VUEig58T5fuFi9ifboBxij", "0001000001189", "Just Testing ", 1);
            //msIOs.SendNotification("cFPtPgzgg1o,APA91bFt7Tp1mb1dthhHQVqDkbQHUapp-O1xrk9rKiLdLbJGJltq32smRjNE1lihbnN7sf5j8Fnjii2-o_MlDap-GB-IyqXBtZkJc4h0XxZzWFGRtOmyT9VUEig58T5fuFi9ifboBxij", "0001000003152", "Just Testing notification", 1);

            //msIOs.SendNotification("cFPtPgzgg1o,APA91bFt7Tp1mb1dthhHQVqDkbQHUapp-O1xrk9rKiLdLbJGJltq32smRjNE1lihbnN7sf5j8Fnjii2-o_MlDap-GB-IyqXBtZkJc4h0XxZzWFGRtOmyT9VUEig58T5fuFi9ifboBxij", "0001000003152", "Just Testing notification", 1);
            //msIOs.SendNotification("cFPtPgzgg1o,APA91bFt7Tp1mb1dthhHQVqDkbQHUapp-O1xrk9rKiLdLbJGJltq32smRjNE1lihbnN7sf5j8Fnjii2-o_MlDap-GB-IyqXBtZkJc4h0XxZzWFGRtOmyT9VUEig58T5fuFi9ifboBxij", "0001000003152", "Just Testing notification", 1);
            try
            {
                string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
                logger.Info(message);
                logger.Info("-------------------------------------------------------");
                List<AmountsMovements> LstObjWall = new List<AmountsMovements>();
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
                        logger.Info("Connection Opened");
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        logger.Info("Obtained data set");
                        if (ds != null && ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                            {
                                DataTable dt = ds.Tables[0];
                                foreach (DataRow dr in dt.Rows)
                                {
                                    obj = new AmountsMovements();
                                    //obj.WineId = Convert.ToInt32(dr["WineId"]);
                                    obj.BarCode = dr["WineBarCode"].ToString();
                                    obj.LabelName = dr["LabelName"].ToString();
                                    //obj.BarCode = dr["Barcode"].ToString();
                                    obj.CustomerId = dr["CustomerId"].ToString();
                                    obj.Token = dr["DeviceToken"].ToString();
                                    if (dr["DeviceType"] != DBNull.Value)
                                        obj.DeviceType = Convert.ToInt32(dr["DeviceType"]);
                                    else
                                        obj.DeviceType = 0;
                                    if (dr["PlantFinal"] != DBNull.Value)
                                        obj.StoreId = Convert.ToInt32(dr["PlantFinal"]);
                                    else
                                        obj.StoreId = 0;
                                    LstObjWall.Add(obj);
                                }
                            }
                            if (ds.Tables.Count > 1)
                            {
                                DataTable dt = ds.Tables[1];
                                foreach (DataRow dr in dt.Rows)
                                {
                                    obj = new AmountsMovements();
                                    //obj.WineId = Convert.ToInt32(dr["WineId"]);
                                    obj.BarCode = dr["WineBarCode"].ToString();
                                    obj.LabelName = dr["LabelName"].ToString();
                                    //obj.BarCode = dr["Barcode"].ToString();
                                    obj.CustomerId = dr["CustomerId"].ToString();

                                    obj.Token = dr["DeviceToken"].ToString();
                                    if (dr["DeviceType"] != DBNull.Value)
                                        obj.DeviceType = Convert.ToInt32(dr["DeviceType"]);
                                    else
                                        obj.DeviceType = 0;
                                    if (dr["PlantFinal"] != DBNull.Value)
                                        obj.StoreId = Convert.ToInt32(dr["PlantFinal"]);
                                    else
                                        obj.StoreId = 0;
                                    LstObjPP.Add(obj);
                                }
                            }

                            MessageSender ms = new MessageSender();
                            MessageSenderIOS msIOs = new MessageSenderIOS();
                            for (int i = 0; i < LstObjWall.Count; i++)
                            {
                                if (LstObjWall[i].Token != null && LstObjWall[i].Token != "")
                                {
                                    if (LstObjWall[i].DeviceType == 1)
                                        ms.SendNotification(LstObjWall[i].Token.Replace(',', ':'), LstObjWall[i].BarCode, LstObjWall[i].LabelName, LstObjWall[i].StoreId);
                                    else if (LstObjWall[i].DeviceType == 2)
                                        msIOs.sendMessage(LstObjWall[i].BarCode, LstObjWall[i].Token, LstObjWall[i].LabelName, LstObjWall[i].StoreId);
                                    logger.Info("Sent notification for WineId:" + LstObjWall[i].BarCode + " for CustomerID:" + LstObjWall[i].CustomerId);
                                }
                                else
                                {
                                    logger.Info("Device token not available for CustomerID:" + LstObjWall[i].CustomerId);
                                }
                                
                            }
                            for (int i = 0; i < LstObjPP.Count; i++)
                            {

                                if (LstObjPP[i].Token != null && LstObjPP[i].Token != "")
                                {
                                    if (LstObjPP[i].DeviceType == 1)
                                        ms.SendNotification(LstObjPP[i].Token.Replace(',', ':'), LstObjPP[i].BarCode, LstObjPP[i].LabelName, LstObjPP[i].StoreId);
                                    else if (LstObjPP[i].DeviceType == 2)
                                        msIOs.sendMessage(LstObjPP[i].BarCode, LstObjPP[i].Token, LstObjPP[i].LabelName, LstObjPP[i].StoreId);
                                    logger.Info("Sent notification for WineId:" + LstObjWall[i].BarCode + " for CustomerID:" + LstObjWall[i].CustomerId);
                                }
                                else
                                {
                                    logger.Info("Device Token not available for CustomerID:" + LstObjWall[i].CustomerId);
                                }
                                
                            }
                        }
                    }
                }
                logger.Info("-------------------------------------------------------");

            }
            catch (Exception ex)
            {
                logger.Trace("Exception caught = " + ex.Message.ToString());
            }
        }
    }
}
