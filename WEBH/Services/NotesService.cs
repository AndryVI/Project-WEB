using Microsoft.AspNetCore.Mvc.Filters;
using System.IO;
using System;
using System.Collections.Generic;
using WebH.Models;
using System.Text;

namespace WebH.Services
{
    public class NotesService
    {
        private static List<Notes> listNotes;
        private WriterAndReadService wArSeirvice = new WriterAndReadService();

        public NotesService()
        {
            Console.OutputEncoding = Encoding.Unicode;

            string[] lines = File.ReadAllLines("App_Data/notes.txt");

            listNotes = new List<Notes>();

            foreach (string line in lines)
            {
                string[] items = line.Split(';');

                Notes notes = new Notes();
                notes.LoginName = items[0].Trim();
                notes.Topic = items[1].Trim();
                notes.DataTime = Convert.ToDateTime(items[2].Trim());
                notes.Context = items[3].Trim();
                

                listNotes.Add(notes);
            }
        }



        public List<Notes> GetNotes(string loginName)
        {
            List<Notes> listNotesByUser = new List<Notes>();

            foreach (var note in listNotes)
                if (loginName == note.LoginName)
                    listNotesByUser.Add(note);

            return listNotesByUser;
        }


        public void AddNotes(Notes newnotes)
        {
            listNotes.Add(newnotes);
            wArSeirvice.FileWriter($"{newnotes.LoginName}; {newnotes.Topic}; {newnotes.DataTime}  ; {newnotes.Context}", getfile: @"\notes.txt", getdirectory: @"App_Data");
        }

    }
}
