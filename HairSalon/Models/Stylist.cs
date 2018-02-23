using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace HairSalon.Models
{
  public class Stylist
  {
    private int _id;
    public string _lastName;
    public string _firstName;
    
    public Stylist(string lastName, string firstName, int id = 0)
    {
      _id = id;
      _lastName = lastName;
      _firstName = firstName;
    }
    
    public override bool Equals(System.Object otherStylist)
    {
      if (!(otherStylist is Stylist))
        return false;
      Stylist newStylist = (Stylist) otherStylist;
      return _lastName == newStylist._lastName && _firstName == newStylist._firstName && _id == newStylist._id;
    }
    public override int GetHashCode()
    {
        return _id.GetHashCode();
    }
    
    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO stylists (last_name, first_name) VALUES (@lastName, @firstName);";
      cmd.Parameters.Add(new MySqlParameter("@lastName", _lastName));
      cmd.Parameters.Add(new MySqlParameter("@firstName", _firstName));
      
      cmd.ExecuteNonQuery();
      _id = (int)cmd.LastInsertedId;
      
      conn.Close();
      if (conn != null)
        conn.Dispose();
    }
    
    public static List<Stylist> GetAll()
    {
      List<Stylist> allStylist = new List<Stylist>();
      
      MySqlConnection conn = DB.Connection();
      conn.Open();
      
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM stylists;";
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      Console.WriteLine("starting reader");
      while(rdr.Read())
      {
        Console.WriteLine("reading");
        int id = rdr.GetInt32(0);
        string lastName = rdr.GetString(1);
        string firstName = rdr.GetString(2);
        Stylist newStylist = new Stylist(lastName, firstName, id);
        allStylist.Add(newStylist);
      }
      
      conn.Close();
      if (conn != null)
        conn.Dispose();
      
      return allStylist;
    }
    
    public static void Clear()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM stylists; ALTER TABLE stylists AUTO_INCREMENT = 1;";

      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
        conn.Dispose();
    }
  }
}