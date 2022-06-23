using Moe.La.Core.Dtos;
using System;
using System.Collections.Generic;

namespace Moe.La.UnitTests.Builders
{
    public class BoardMeetingBuilder
    {
        BoardMeetingDto _boardMeetingDto = new BoardMeetingDto();

        public BoardMeetingBuilder BoardId(int boardId)
        {
            _boardMeetingDto.BoardId = boardId;
            return this;
        }
        public BoardMeetingBuilder MemoId(int memoId)
        {
            _boardMeetingDto.MemoId = memoId;
            return this;
        }
        public BoardMeetingBuilder BoardMeetingMembers(List<int> boardMeetingMembers)
        {
            _boardMeetingDto.BoardMeetingMembers = boardMeetingMembers;
            return this;
        }
        public BoardMeetingBuilder WithDefaultValues()
        {
            _boardMeetingDto = new BoardMeetingDto
            {
                BoardId = 1,
                MemoId = 1,
                MeetingDate = DateTime.Now,
                MeetingPlace = "Meeting Hall"
            };

            return this;
        }

        public BoardMeetingDto Build() => _boardMeetingDto;
    }
}
