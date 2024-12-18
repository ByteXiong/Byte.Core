import { getCurrentInstance, h, ref } from 'vue';
import * as Naive from 'naive-ui';
import dayjs from 'dayjs';
import { $t } from '@/locales';
import * as El from '@/api/apiEls';
import * as Enum from '@/api/apiEnums';
import { useAuth } from '@/hooks/business/auth';
import type { TableColumn } from '@/api/globals';
const customRender = (column: TableColumn) => {
  const currentCpn = getCurrentInstance();
  const props = ref<Naive.DataTableColumn>({
    key: column.key || '',
    render:
  });
  if (!column.props) {
    switch (column.columnType) {
      case Enum.ColumnTypeEnum.日期:
        props.value.render = row => {
          return dayjs(row[column.key || '']).format('YYYY-MM-DD');
        };
        break;
      case Enum.ColumnTypeEnum.时间:
        if (val) {
          return dayjs(val).format('YYYY-MM-DD HH:mm:ss');
        }
        return '';
      case Enum.ColumnTypeEnum.时间戳转当地日期:
        if (val) {
          return dayjs.unix(val).format('YYYY-MM-DD');
        }
        return '';
      case Enum.ColumnTypeEnum.时间戳转当地时间:
        if (val) {
          dayjs.unix(val).format('YYYY-MM-DD HH:mm:ss');
        }
        return '';
      // case Enum.ColumnTypeEnum.单图:
      //   if (val) {
      //   }
      default:
        break;
    }
  } else {
    // eslint-disable-next-line consistent-this, @typescript-eslint/no-unused-vars
    const that = currentCpn?.exposed;
    Object.defineProperty(window, 'that', {
      value: that,
      configurable: true,
      enumerable: false, // 不会出现在 Object.keys 中
      writable: true
    });
    const { hasAuth } = useAuth();

    Object.defineProperty(window, 'hasAuth', {
      value: hasAuth,
      configurable: true,
      enumerable: false, // 不会出现在 Object.keys 中
      writable: true
    });

    Object.defineProperty(window, 'h', {
      value: h,
      configurable: true,
      enumerable: false, // 不会出现在 Object.keys 中
      writable: true
    });
    // eslint-disable-next-line @typescript-eslint/no-unused-vars

    Object.defineProperty(window, 'Naive', {
      value: Naive,
      configurable: true,
      enumerable: false, // 不会出现在 Object.keys 中
      writable: true
    });
    // eslint-disable-next-line @typescript-eslint/no-unused-vars
    Object.defineProperty(window, 'El', {
      value: El,
      configurable: true,
      enumerable: false, // 不会出现在 Object.keys 中
      writable: true
    });
    // eslint-disable-next-line @typescript-eslint/no-unused-vars
    Object.defineProperty(window, '$t', {
      value: $t,
      configurable: true,
      enumerable: false, // 不会出现在 Object.keys 中
      writable: true
    });
    // eslint-disable-next-line @typescript-eslint/no-unused-vars
    Object.defineProperty(window, 'Enum', {
      value: Enum,
      configurable: true,
      enumerable: false, // 不会出现在 Object.keys 中
      writable: true
    });

    Object.defineProperty(window, 'dayjs', {
      value: dayjs,
      configurable: true,
      enumerable: false, // 不会出现在 Object.keys 中
      writable: true
    });
    // eslint-disable-next-line no-eval
    return eval(`(${column.props || '{}'})`);
  }
};
export default customRender;
