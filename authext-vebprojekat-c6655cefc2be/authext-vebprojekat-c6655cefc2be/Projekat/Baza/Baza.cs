﻿using Projekat.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Microsoft.Ajax.Utilities;

namespace Projekat.Baza
{
    public class Baza : IBaza
    {
        public Baza(SQLiteConnection conn)
        {
            _conn = conn;
        }

        // User related

      

        public bool CanLogIn(string username, string password)
        {
            using (var cmd = _conn.CreateCommand())
            {
                cmd.CommandText =
                    @"SELECT COUNT()
                      FROM Korisnici
                      WHERE Korisnicko_Ime = @name AND Lozinka = @password
                      LIMIT 1;";
                cmd.Parameters.AddWithValue("@name", username);
                cmd.Parameters.AddWithValue("@password", password);

                using (var reader = cmd.ExecuteReader())
                {
                    reader.Read();

                    return reader.GetInt32(0) == 1;
                }
            }
        }

        public bool Register(Korisnik user)
        {
            using (var cmd = _conn.CreateCommand())
            {
                cmd.CommandText =
                    @"INSERT INTO Korisnici(Korisnicko_Ime, Lozinka, Ime, Prezime, Uloga_Korisnika, Pol, JMBG, Telefon, Email, Lokacija, Automobil)
                      VALUES (@name, @password, @firstname, @lastname, @role ,@pol, @jmbg, @phoneno, @email, NULL, NULL);";
                cmd.Parameters.AddWithValue("@name", user.Korisnicko_Ime);
                cmd.Parameters.AddWithValue("@password", user.Lozinka);
                cmd.Parameters.AddWithValue("@firstname", user.Ime);
                cmd.Parameters.AddWithValue("@lastname", user.Prezime);
                cmd.Parameters.AddWithValue("@role", (int)user.Uloga_Korisnika);
                cmd.Parameters.AddWithValue("@pol", user.Pol);
                cmd.Parameters.AddWithValue("@jmbg", user.JMBG);
                cmd.Parameters.AddWithValue("@phoneno", user.Telefon);
                cmd.Parameters.AddWithValue("@email", user.Email);

                return Execute(cmd);
            }
        }

        
        public Korisnik GetUser(string username)
        {
            using (var cmd = _conn.CreateCommand())
            {
                cmd.CommandText =
                    @"SELECT Ime, Prezime, Uloga_Korisnika, Pol, Email, JMBG, Telefon
                      FROM Korisnici
                      WHERE Korisnicko_Ime = @name
                      LIMIT 1;";
                cmd.Parameters.AddWithValue("@name", username);

                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                        return null;

                    return new Korisnik()
                    {
                        Korisnicko_Ime = username,
                        Ime = reader.GetString(0),
                        Prezime = reader.GetString(1),
                        Uloga_Korisnika = (Korisnik.Uloga)reader.GetInt32(2),
                        Telefon = reader.GetString(6),
                        Email = reader.GetString(4),
                        Pol = reader.GetString(3),
                        JMBG = reader.GetString(5)

                    };
                }
            }
        }

      


        private readonly SQLiteConnection _conn;

        private static bool Execute(SQLiteCommand cmd)
        {
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (SQLiteException)
            {
                return false;
            }

            return true;
        }

        bool IBaza.EditUser(string korisnickoime, string ime, string prezime, string telefon, string pol, string jmbg, string email)
        {
            using (var cmd = _conn.CreateCommand())
            {
                cmd.CommandText =
                    @"UPDATE Korisnici
                      SET Ime = @ime, Prezime = @prezime, Pol = @pol, JMBG = @jmbg, Telefon = @telefon, Email = @email
                      WHERE Korisnicko_Ime = @korisnickoime";
                cmd.Parameters.AddWithValue("@ime", ime);
                cmd.Parameters.AddWithValue("@prezime", prezime);
                cmd.Parameters.AddWithValue("@telefon", telefon);
                cmd.Parameters.AddWithValue("@pol", pol);
                cmd.Parameters.AddWithValue("@jmbg", jmbg);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@korisnickoime", korisnickoime);

                return Execute(cmd);
            }
        }
    }
}