using Moe.La.Core.Dtos;

namespace Moe.La.UnitTests.Builders
{
    class GeneralManagementBuilder
    {
        private BranchDto _generalManagement = new BranchDto();

        public GeneralManagementBuilder Name(string name)
        {
            _generalManagement.Name = name;
            return this;
        }
        public GeneralManagementBuilder ParentId(int parentId)
        {
            _generalManagement.ParentId = parentId;
            return this;
        }

        public GeneralManagementBuilder WithDefaultValues()
        {
            _generalManagement = new BranchDto
            {
                Name = "اختبار 1"
            };

            return this;
        }
        public BranchDto Build() => _generalManagement;
    }
}
