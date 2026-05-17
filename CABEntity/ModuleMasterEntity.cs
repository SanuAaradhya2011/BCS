using CAB.Framework.Entity;

namespace CAB.Entity
{
	public class ModuleMasterEntity : EntityBase
	{
		private int _module_ID;
		private int _module_Name;

		public int Module_ID
		{
			get
			{
				return _module_ID;
			}
			set
			{
				_module_ID = value;
			}
		}

		public int Module_Name
		{
			get
			{
				return _module_Name;
			}
			set
			{
				_module_Name = value;
			}
		}
	}
}
