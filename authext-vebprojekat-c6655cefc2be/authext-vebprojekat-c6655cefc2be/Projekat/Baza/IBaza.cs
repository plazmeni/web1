﻿using System;
using Projekat.Models;
using System.Collections.Generic;

namespace Projekat.Baza
{
    public interface IBaza
    {
     
        bool CanLogIn(string username, string password);
        bool Register(Korisnik user);
        

        Korisnik GetUser(string username);
        
       
     

      


        bool EditUser(string korisnickoime, string ime, string prezime, string telefon, string pol, string jmbg, string email);


    }
}