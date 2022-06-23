﻿using Moe.La.Core.Dtos;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface IMoeenCaseLogRepository
    {
        Task AddAsync(MoeenCaseDto moeenCaseDto);
    }
}