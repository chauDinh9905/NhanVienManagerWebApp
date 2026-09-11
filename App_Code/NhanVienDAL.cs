using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace NhanVienWebApp.App_Code
{
    public class NhanVien
    {
        public int ID { get; set; }
        public string Account { get; set; }
        public string HoTen { get; set; }
        public string QueQuan { get; set; }
        public DateTime NgaySinh { get; set; }
        public int GioiTinh { get; set; }
        public string TruongHoc { get; set; }
    }
    public static class NhanVienDAL
    {
        public static DataTable GetAll(string tableName)
        {
            var dt = new DataTable();
            using (var conn = DbSessionHelper.CreateDbConnection())
            {
                conn.Open();
                // tableName lấy từ Session (đã được kiểm tra tồn tại qua SchemaHelper), không do người dùng gõ trực tiếp vào query
                string sql = "SELECT ID, Account, HoTen, QueQuan, NgaySinh, GioiTinh, TruongHoc FROM `" + tableName + "` ORDER BY ID";
                using (var da = new MySqlDataAdapter(sql, conn))
                {
                    da.Fill(dt);
                }
            }
            return dt;
        }
        public static void Insert(string tableName, NhanVien nv)
        {
            using (var conn = DbSessionHelper.CreateDbConnection())
            {
                conn.Open();
                string sql = "INSERT INTO `" + tableName +
                    "` (Account, HoTen, QueQuan, NgaySinh, GioiTinh, TruongHoc) " +
                    "VALUES (@account, @hoten, @quequan, @ngaysinh, @gioitinh, @truonghoc)";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@account", nv.Account);
                    cmd.Parameters.AddWithValue("@hoten", nv.HoTen);
                    cmd.Parameters.AddWithValue("@quequan", nv.QueQuan);
                    cmd.Parameters.AddWithValue("@ngaysinh", nv.NgaySinh);
                    cmd.Parameters.AddWithValue("@gioitinh", nv.GioiTinh);
                    cmd.Parameters.AddWithValue("@truonghoc", nv.TruongHoc);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static void Update(string tableName, NhanVien nv)
        {
            using (var conn = DbSessionHelper.CreateDbConnection())
            {
                conn.Open();
                string sql = "UPDATE `" + tableName + "` SET Account=@account, HoTen=@hoten, QueQuan=@quequan, " +
                    "NgaySinh=@ngaysinh, GioiTinh=@gioitinh, TruongHoc=@truonghoc WHERE ID=@id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@account", nv.Account);
                    cmd.Parameters.AddWithValue("@hoten", nv.HoTen);
                    cmd.Parameters.AddWithValue("@quequan", nv.QueQuan);
                    cmd.Parameters.AddWithValue("@ngaysinh", nv.NgaySinh);
                    cmd.Parameters.AddWithValue("@gioitinh", nv.GioiTinh);
                    cmd.Parameters.AddWithValue("@truonghoc", nv.TruongHoc);
                    cmd.Parameters.AddWithValue("@id", nv.ID);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static NhanVien GetById(string tableName, int id)
        {
            using (var conn = DbSessionHelper.CreateDbConnection())
            {
                conn.Open();
                string sql = "SELECT ID, Account, HoTen, QueQuan, NgaySinh, GioiTinh, TruongHoc FROM `" + tableName + "` WHERE ID=@id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new NhanVien
                            {
                                ID = reader.GetInt32(0),
                                Account = reader.GetString(1),
                                HoTen = reader.GetString(2),
                                QueQuan = reader.GetString(3),
                                NgaySinh = reader.GetDateTime(4),
                                GioiTinh = reader.GetInt32(5),
                                TruongHoc = reader.GetString(6)
                            };
                        }
                    }
                }
            }
            return null;
        }
        public static int DeleteMany(string tableName, List<int> ids)
        {
            if (ids == null || ids.Count == 0) return 0;
            using (var conn = DbSessionHelper.CreateDbConnection())
            {
                conn.Open();
                var paramNames = new List<string>();
                using (var cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    for (int i = 0; i < ids.Count; i++)
                    {
                        string pName = "@id" + i;
                        paramNames.Add(pName);
                        cmd.Parameters.AddWithValue(pName, ids[i]);
                    }
                    cmd.CommandText = "DELETE FROM `" + tableName + "` WHERE ID IN (" + string.Join(",", paramNames) + ")";
                    return cmd.ExecuteNonQuery();
                }
            }
        }
    }
}