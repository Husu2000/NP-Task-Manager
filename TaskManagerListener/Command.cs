using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerListener;

public class Command
{

	public const string processList = "PROCLIST";
	public const string kill = "KILL";
	public const string run = "RUN";
	public string? Text { get; set; }
	public string? Param { get; set; }




}

