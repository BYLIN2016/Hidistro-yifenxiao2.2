namespace Hidistro.Entities.Distribution
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class DistributorQuery : Pagination
    {
        
        private int? _GradeId;
        
        private bool _IsApproved;
        
        private int? _LineId;
        
        private string _RealName;
        
        private string _Username;

        public int? GradeId
        {
            
            get
            {
                return _GradeId;
            }
            
            set
            {
                _GradeId = value;
            }
        }

        public bool IsApproved
        {
            
            get
            {
                return _IsApproved;
            }
            
            set
            {
                _IsApproved = value;
            }
        }

        public int? LineId
        {
            
            get
            {
                return _LineId;
            }
            
            set
            {
                _LineId = value;
            }
        }

        public string RealName
        {
            
            get
            {
                return _RealName;
            }
            
            set
            {
                _RealName = value;
            }
        }

        public string Username
        {
            
            get
            {
                return _Username;
            }
            
            set
            {
                _Username = value;
            }
        }
    }
}

