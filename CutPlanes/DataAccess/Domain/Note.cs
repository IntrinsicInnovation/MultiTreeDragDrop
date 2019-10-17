using System;
using System.Collections.Generic;
using EpicDataAccess.Domain;
using System.Linq;

namespace EpicDataAccess.Domain
{
    public partial class Note 
    {   

        public string Details
        {
            get { return this.Note1; }
        }

        public int TypeId
        {
            get { return this.NotesTypeId; }
        }

        public int AuthorId
        {
            get { return this.UserCreatedId; }
        }

        #region [Constructors]

        public Note(EpicDataAccess.ProjectManagement.Note projectNote)
        {
            this.DatedCreated = projectNote.DatedCreated;
            this.DocumentUri = projectNote.DocumentUri;
            this.FileReferenceId = projectNote.FileReferenceId;
            this.Id = projectNote.Id;
            this.IsDeleted = projectNote.IsDeleted;
            this.Note1 = projectNote.Note1;
            this.NotesLinkId = projectNote.NotesLinkId;
            this.NotesTypeId = projectNote.NotesTypeId;
        }

        #endregion

        //public string Resource
        //{
        //    get { return "Plant"; }
        //}

        //public int ResourceId
        //{
        //    get { return this.LinkNotes.First().PlantId.Value; }
        //}
    }
}
