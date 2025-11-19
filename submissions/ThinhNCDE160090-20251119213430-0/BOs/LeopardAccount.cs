using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BOs;

public partial class LeopardAccount
{
    [Key]
    public int AccountId { get; set; }

    public string UserName { get; set; } = null!;

    public string? Password { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public int? RoleId { get; set; }
}
