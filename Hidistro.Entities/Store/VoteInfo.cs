namespace Hidistro.Entities.Store
{
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class VoteInfo
    {
        
        private bool _IsBackup;
        
        private int _MaxCheck;
        
        private int _VoteCounts;
        
        private long _VoteId;
        
        private IList<VoteItemInfo> _VoteItems;
        
        private string _VoteName;

        public bool IsBackup
        {
            
            get
            {
                return _IsBackup;
            }
            
            set
            {
                _IsBackup = value;
            }
        }

        [RangeValidator(1, RangeBoundaryType.Inclusive, 100, RangeBoundaryType.Inclusive, Ruleset="ValVote", MessageTemplate="最多可选项数不允许为空，范围为1-100之间的整数")]
        public int MaxCheck
        {
            
            get
            {
                return _MaxCheck;
            }
            
            set
            {
                _MaxCheck = value;
            }
        }

        public int VoteCounts
        {
            
            get
            {
                return _VoteCounts;
            }
            
            set
            {
                _VoteCounts = value;
            }
        }

        public long VoteId
        {
            
            get
            {
                return _VoteId;
            }
            
            set
            {
                _VoteId = value;
            }
        }

        public IList<VoteItemInfo> VoteItems
        {
            
            get
            {
                return _VoteItems;
            }
            
            set
            {
                _VoteItems = value;
            }
        }

        [StringLengthValidator(1, 60, Ruleset="ValVote", MessageTemplate="投票调查的标题不能为空，长度限制在60个字符以内")]
        public string VoteName
        {
            
            get
            {
                return _VoteName;
            }
            
            set
            {
                _VoteName = value;
            }
        }
    }
}

