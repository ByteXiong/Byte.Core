import { DeptTypeEnum, MenuTypeEnum } from './apiEnums';

export const MenuTypeEl: Record<number, NaiveUI.ThemeColor> = {
  [MenuTypeEnum.菜单]: 'warning',
  [MenuTypeEnum.目录]: 'success',
  [MenuTypeEnum.按钮]: 'error',
  [MenuTypeEnum.外链]: 'info'
};

export const DeptTypeEl: Record<number, 'success' | 'warning' | 'info' | 'danger'> = {
  [DeptTypeEnum.公司]: 'warning',
  [DeptTypeEnum.平台]: 'success',
  [DeptTypeEnum.部门]: 'danger'
};
