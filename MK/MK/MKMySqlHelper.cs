using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static MK.DataClass;

namespace MK
{
    public class MKMySqlHelper
    {
        public static MySqlConnection gMySQLServerCon { get; private set; }
        public static string SqlSheet { get; internal set; }
        public static string MySQLconStr { get; internal set; }
        public static void SetSqlSheet(string str) {
            SqlSheet = str;
        }
        public static void SetMySQLconStr(string str) {
            MySQLconStr = str;
        }



        internal static MySqlConnection getsqlCon()
        {
            if (String.IsNullOrWhiteSpace(MySQLconStr)) {
                MessageBox.Show("MySQLconStr NULL");
                return null;
            }
            try
            {
                if (gMySQLServerCon == null || gMySQLServerCon.State != ConnectionState.Open)
                {
                    LogHelper.Log("start New SqlserverConnection: " );
                    gMySQLServerCon = new MySqlConnection(MySQLconStr);
                    gMySQLServerCon.Open();
                    LogHelper.Log("finish New SqlserverConnection: ");
                }
                return gMySQLServerCon;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
                return null;
            }
        }

        public static DataTable SqlInitGrid(DataGrid dataGrid, string sql)
        {
            MySqlConnection mMySqlConnection = getsqlCon();
            if (mMySqlConnection == null) { return null; }

            DataTable dt = Getdt(sql, getsqlCon());
            dataGrid.ItemsSource = dt.DefaultView;
            return dt;
        }

        public static void SqlInitComboBox(ComboBox mComboBox, string sql, string Displayname, string Value, int t = -1)
        {
            MySqlConnection mMySqlConnection = getsqlCon();
            if (mMySqlConnection == null) { return; }
            DataSet Ds = Getds(sql, getsqlCon());
            mComboBox.DisplayMemberPath = Displayname;
            mComboBox.SelectedValuePath = Value;
            mComboBox.ItemsSource = Ds.Tables["databases"].DefaultView;
            mComboBox.SelectedIndex = t;
            return;
        }

        private static DataTable Getdt(string sql, MySqlConnection sqlConnection)
        {
            MySqlConnection mMySqlConnection = getsqlCon();
            if (mMySqlConnection == null) return null;
            if (sqlConnection == null) return null;
            DataTable dt = new DataTable();
            MySqlDataAdapter sda = new MySqlDataAdapter(sql, getsqlCon());
            sda.Fill(dt);
            return dt;
        }

        private static DataSet Getds(string sql, MySqlConnection sqlConnection)
        {
            if (sqlConnection == null) return null;
            DataSet ds = new DataSet();
            MySqlDataAdapter sda = new MySqlDataAdapter(sql, sqlConnection);
            sda.Fill(ds, "databases");
            return ds;
        }

        public static List<T> GetList<T>(string sql) where T : class
        {
            MySqlConnection mMySqlConnection = getsqlCon();
            if (mMySqlConnection == null) return null;
            try
            {
                MySqlCommand SqlCommand = new MySqlCommand(sql, getsqlCon());
                DbDataReader mySQLReader = (MySqlDataReader)SqlCommand.ExecuteReader();
                List<T> list = GetListob<T>(mySQLReader);
                mySQLReader.Close();
                return list;
            }
            catch (Exception ee)
            {

                MessageBox.Show(ee.ToString());
                return null;
            }
        }
        public static User Login(string username, string password)
        {
            try
            {
                List<User> t1 = GetList<User>("select * from sys_user where name = '" + username + "' and password = '" + password + "'");
                if (t1 == null || t1.Count == 0)
                {
                    MessageBox.Show("User Password wrong");
                    return null;
                }
                else
                {
                    return t1[0];
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("请联网 ！！" + ee.ToString());
                Trace.WriteLine(ee.ToString());
                return null;
            }
        }

        public static int SqlEx(List<MySqlParameter> list, string sql)
        {
            return update(sql, list.ToArray());
        }

        public static int update(string sql, params MySqlParameter[] ps)
        {
            MySqlConnection mMySqlConnection = getsqlCon();
            if (mMySqlConnection == null) return -1;
            try
            {
                MySqlCommand cmd = new MySqlCommand(sql, mMySqlConnection);
                cmd.Parameters.AddRange(ps);
                int re = cmd.ExecuteNonQuery();
                return re;
            }
            catch (Exception)
            {
                return -1;
            }
        }


        public static int SqlEx(string sql)
        {
            return update(sql);
        }

        public static int update(string sql)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(sql, getsqlCon());
                return GetCmdEx(cmd);
            }
            catch (Exception)
            {
                return -1;
            }

        }

        private static int GetCmdEx(MySqlCommand cmd)
        {
            return cmd.ExecuteNonQuery();
        }

        public static List<T> GetListob<T>(DbDataReader reader) where T : class
        {
            List<T> list = new List<T>();
            Type type = typeof(T);
            PropertyInfo[] p = type.GetProperties(); //得到该T类中的所有公共属性

            while (reader.Read())
            {
                var model = Activator.CreateInstance<T>();
                foreach (var item in p)
                {
                    if (item == null)
                    {
                        continue;
                    }

                    try
                    {
                        item.SetValue(model, reader[item.Name], null);
                    }
                    catch (Exception)
                    {

                        item.SetValue(model, null, null);
                    }
                }
                list.Add(model);
            }
            reader.Close();
            return list;
        }

        public static int AddColumn(string columnName)
        {

            string ssql = "alter table " + SqlSheet + " add " + columnName + " varchar(50) NULL";
            return update(ssql);
        }

        public static int AddItem(string item, string md5, List<data1Info> mdataList, int fileSizeK)
        {

            List<MySqlParameter> list = new List<MySqlParameter>();
            string sql = "insert  into " + SqlSheet + " set ";
            string sql2 = " ItemName=@ItemName ";
            foreach (data1Info item1 in mdataList)
            {
                sql2 += " , " + item1.name + "=@" + item1.name + " ";
                list.Add(new MySqlParameter(item1.name, item1.value.Trim()));
            }
            list.Add(new MySqlParameter("ItemName", item));
            string[] xx = item.Split('.');
            list.Add(new MySqlParameter("FileType", xx[xx.Length - 1]));
            list.Add(new MySqlParameter("FileSize", fileSizeK + "K"));
            list.Add(new MySqlParameter("MD5", md5));
            sql2 += " , FileType=@FileType , FileSize=@FileSize,MD5=@MD5";
            sql += sql2;



            return MK.MKMySqlHelper.SqlEx(list, sql);

        }

        public static DataItem GetExistItem(string item)
        {

          
            string sql = "select ItemName from " + SqlSheet + " where MD5 = '"+ item + "'";


            try
            {
                List<DataItem> t1 = GetList<DataItem>("select ItemName from " + SqlSheet + " where MD5 = '" + item + "'");
                if (t1 == null || t1.Count == 0)
                {
                  //  MessageBox.Show("User Password wrong");
                    return null;
                }
                else
                {
                    return t1[0];
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("请联网 ！！" + ee.ToString());
                Trace.WriteLine(ee.ToString());
                return null;
            }

        }
        

    }

    
}
