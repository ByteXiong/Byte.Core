import { getCurrentInstance, h } from 'vue';
import * as Naive from 'naive-ui';
import { $t } from '@/locales';
import * as El from '@/api/apiEls';
import * as Enum from '@/api/apiEnums';
import { useAuth } from '@/hooks/business/auth';

const customRender = (str: string | undefined) => {
  const currentCpn = getCurrentInstance();

  // eslint-disable-next-line consistent-this, @typescript-eslint/no-unused-vars
  const that = currentCpn?.exposed;

  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  const { hasAuth } = useAuth();

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
  // eslint-disable-next-line no-eval
  return eval(`(${str || '{}'})`);
};
export default customRender;
