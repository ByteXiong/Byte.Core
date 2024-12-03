export enum VersionEnum {
  def = 0,
  /// <summary>
  /// Pc端
  /// </summary>
  Pc = 1,
  /// <summary>
  ///
  /// </summary>
  App = 2
}
export enum StateEnum {
  /// <summary>
  /// 删除
  /// </summary>
  del = -1,
  /// <summary>
  /// 正常
  /// </summary>
  normal = 0,
  /// <summary>
  /// 新增
  /// </summary>
  add = 1,
  /// <summary>
  /// 更新
  /// </summary>
  update = 2
}
export enum OrderType {
  /// <summary>
  /// 正序
  /// </summary>
  asc = 1,
  /// <summary>
  /// 倒序
  /// </summary>
  desc = 2
}

export enum ViewTypeEnum {
  主页 = 1,
  编辑页 = 2,
  详情页 = 3
}

export enum MenuTypeEnum {
  目录 = 1,
  菜单 = 2,
  按钮 = 3,
  参数 = 4
}
export enum RoleTypeEnum {
  系统角色 = 10,
  公司角色 = 20,
  部门角色 = 30,
  个人角色 = 40
}

export enum DeptTypeEnum {
  平台 = 10,

  公司 = 20,

  部门 = 30
}
/// <summary>
/// 对齐方式
/// </summary>
export enum TableAlignEnum {
  left = 1,
  center,
  right
}
/// <summary>
/// 固定方式
/// </summary>
export enum TableFixedEnum {
  left = 1,
  '' = 2,
  right = 3
}

export enum SearchTypeEnum {
  等于,
  模糊,
  大于,
  大于或等于,
  小于,
  小于或等于,
  区间
}

export enum ColumnTypeEnum {
  整数 = 1,
  文本 = 2,
  枚举 = 3,
  字典 = 4,
  小数 = 5,
  日期 = 6,
  时间 = 7,
  时间戳转当地日期 = 8,
  时间戳转当地时间 = 9,
  单图 = 10,
  多图 = 11,
  文件 = 12,
  布尔 = 13,
  颜色 = 14,
  自定义 = 99
}

export enum LayoutTypeEnum {
  Base = 1,
  Blank = 2
}
export enum IconTypeEnum {
  iconify图标 = 1,
  本地图标 = 2
}

export enum JobActionEnum {
  启动 = 1,
  暂停 = 2,
  重启 = 3,
  执行 = 4
}

/// <summary>
/// 作业触发器状态
/// </summary>
export enum TriggerStateEnum {
  /// <summary>
  /// 积压 Backlog
  /// </summary>
  /// <remarks>起始时间大于当前时间</remarks>
  Backlog = 1,

  /// <summary>
  /// 就绪
  /// </summary>
  Ready = 2,

  /// <summary>
  /// 正在运行
  /// </summary>
  Running = 3,

  /// <summary>
  /// 暂停
  /// </summary>
  Paused = 4,

  /// <summary>
  /// 阻塞
  /// </summary>
  /// <remarks>本该执行但是没有执行</remarks>
  Blocked = 5,

  /// <summary>
  /// 由失败进入就绪
  /// </summary>
  /// <remarks>运行错误当并未超出最大错误数，进入下一轮就绪</remarks>
  ErrorToReady = 6,

  /// <summary>
  /// 归档
  /// </summary>
  /// <remarks>结束时间小于当前时间</remarks>
  Complete = 7,

  /// <summary>
  /// 崩溃
  /// </summary>
  /// <remarks>错误次数超出了最大错误数</remarks>
  Panic = 8,

  /// <summary>
  /// 超限
  /// </summary>
  /// <remarks>运行次数超出了最大限制</remarks>
  Overrun = 9,

  /// <summary>
  /// 无触发时间
  /// </summary>
  /// <remarks>下一次执行时间为 null </remarks>
  Unoccupied = 10,

  /// <summary>
  /// 未启动
  /// </summary>
  NotStart = 11,

  /// <summary>
  /// 未知作业触发器
  /// </summary>
  /// <remarks>作业触发器运行时类型为 null</remarks>
  Unknown = 12,

  /// <summary>
  /// 未知作业处理程序
  /// </summary>
  /// <remarks>作业处理程序类型运行时类型为 null</remarks>
  Unhandled = 13
}
