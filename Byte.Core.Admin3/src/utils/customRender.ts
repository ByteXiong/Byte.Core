/* eslint-disable @typescript-eslint/no-unused-vars */
import { getCurrentInstance, h, nextTick } from 'vue';
import * as Naive from 'naive-ui';
import { $t } from '@/locales';
import * as El from '@/api/apiEls';
import * as Enum from '@/api/apiEnums';
import { useAuth } from '@/hooks/business/auth';

const customRender = (str: string | undefined) => {
  const currentCpn = getCurrentInstance();

  // eslint-disable-next-line consistent-this
  const that = currentCpn?.exposed;
  const h1 = h;
  const { hasAuth } = useAuth();
  const Naive1 = Naive;
  const t = $t;
  const El1 = El;
  const Enum1 = Enum;
  // eslint-disable-next-line no-eval
  return eval(`(${str || '{}'})`);
};
export default customRender;
