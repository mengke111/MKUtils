using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static MK.DataClass;

namespace MK
{

    public class UsedInfo
    {
       

        public int downloadCount { get; internal set; }
    }




    public class MKMySqlHelper
    {
        public static MySqlConnection gMySQLServerCon { get; private set; }
        public static string SqlSheet { get; internal set; }
        public static string MySQLconStr { get; internal set; }
        public static void SetSqlSheet(string str)
        {
            SqlSheet = str;
            LogHelper.Log("SqlSheet: " + str);
        }
        public static void SetMySQLconStr(string str)
        {
            MySQLconStr = str;
        }

        internal static MySqlConnection getsqlCon()
        {
            if (String.IsNullOrWhiteSpace(MySQLconStr))
            {
                MessageBox.Show("MySQLconStr NULL");
                return null;
            }
            try
            {
                if (gMySQLServerCon == null || gMySQLServerCon.State != ConnectionState.Open)
                {
                    LogHelper.Log("start New SqlserverConnection: ");
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

        private static DataSet Getds(string sql)
        {
            MySqlConnection sqlConnection = getsqlCon();
            if (sqlConnection == null) return null;
            DataSet ds = new DataSet();
            MySqlDataAdapter sda = new MySqlDataAdapter(sql, sqlConnection);
            sda.Fill(ds, "databases");
            return ds;
        }

        //private static DataSet GetInfo(string ssql)
        //{
        //    MySqlDataAdapter sda = new MySqlDataAdapter(ssql, getsqlCon());
        //    DataSet ds = new DataSet();
        //    sda.Fill(ds);
        //}

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


        public static int AddProject(string SqlSheet)
        {

            string ssql = "CREATE TABLE sys_" + SqlSheet + " LIKE sys_base";
            return update(ssql);
        }
        public static UsedInfo GetUsedInfoNum(string SqlSheet)
        {

            string ssql = "SELECT COUNT(*) FROM sys_download_log";
            DataSet xxxs = Getds(ssql);
            int rowCount = int.Parse(xxxs.Tables[0].Rows[0][0].ToString());
            UsedInfo xUsedInfo = new UsedInfo();
            xUsedInfo.downloadCount = rowCount;
            return xUsedInfo;
        }

       

        public static int AddItem(string item, string md5, List<data1Info> mdataList, int fileSizeK)
        {
            LogHelper.Log(item + " " + md5 + " " + mdataList.Count + " " + fileSizeK);
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
            list.Add(new MySqlParameter("FileSizeK", fileSizeK));
            list.Add(new MySqlParameter("MD5", md5));
            sql2 += " , FileType=@FileType , FileSizeK=@FileSizeK,MD5=@MD5";
            sql += sql2;



            return MK.MKMySqlHelper.SqlEx(list, sql);

        }
        public static int AddDownloadLog(string ProjectText, string mUser, string FTPPath)
        {

            List<MySqlParameter> list = new List<MySqlParameter>();
            string sql = "insert  into sys_download_log  set ";


            list.Add(new MySqlParameter("project", ProjectText));

            list.Add(new MySqlParameter("name", FTPPath));
            list.Add(new MySqlParameter("user", mUser));

            string sql2 = "project=@project,  name=@name , user=@user";
            sql += sql2;

            LogHelper.OnlyLog("sql: " + sql);

            return MK.MKMySqlHelper.SqlEx(list, sql);
        }
        public static DataItem GetExistItem(string item, string selectitems, string itemname)
        {
            try
            {
                List<DataItem> t1 = GetList<DataItem>("select " + selectitems + " from " + SqlSheet + " where " + itemname + " = '" + item + "'");
                if (t1 == null || t1.Count == 0)
                {
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
        public static string DownloadDBMBLFile(string picname, string picpath)
        {
            if (File.Exists(picpath))
            {
                return null;
            }
            LogHelper.Log("NetWork.DownloadDBMBLFile" + picname + "  " + picpath);
            if (picname == null || picname == null || picpath == "" || picpath == "")
            {
                return null;
            }
            try
            {
                string sql = "select binarydata from Icon  where Name = '" + picname + "'";
                byte[] image;

                MySqlCommand command = new MySqlCommand(sql, getsqlCon());//查询语句根据需要修改
                image = (byte[])command.ExecuteScalar();

                if (image == null || image.Length < 200)
                {
                    MessageBox.Show(picname + "图片下载失败");
                    return null;
                }
                //指定从数据库读取出来的图片的保存路径及名字
                // picpath = AppDomain.CurrentDomain.BaseDirectory + @"\Icon\" + picpath;
                //按上面的路径与名字保存图片文件
                BinaryWriter bw = new BinaryWriter(File.Open(picpath, FileMode.OpenOrCreate));
                bw.Write(image);
                bw.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("DownloadDBMBLFile: " + picname + "  " + e.ToString());
                return null;
            }
            return picpath;
        }

    }


}
