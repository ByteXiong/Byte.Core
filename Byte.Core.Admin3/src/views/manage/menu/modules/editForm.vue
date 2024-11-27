<script setup lang="tsx">
import { computed, ref } from 'vue';
import type { SelectOption } from 'naive-ui';
import { useForm, useRequest } from 'alova/client';

import { useNaiveForm } from '@/hooks/common/form';
import { $t } from '@/locales';
import type { MenuButton, MenuQuery, UpdateMenuParam } from '@/api/globals';
import { getLocalIcons } from '@/utils/icon';
import SvgIcon from '@/components/custom/svg-icon.vue';
import { IconTypeEnum, LayoutTypeEnum, MenuTypeEnum } from '@/api/apiEnums';
import { getEnumValue } from '@/utils/common';

defineOptions({
  name: 'MenuEditForm'
});
type FormDataType = UpdateMenuParam;

const visible = ref<boolean>(false);

const { formRef, validate, restoreValidation } = useNaiveForm();
// 规则验证获取对象
// const { defaultRequiredRule } = useFormRules();
type RuleKey = keyof FormDataType;
const rules: Partial<Record<RuleKey, App.Global.FormRule>> = {};

interface Emits {
  (e: 'refresh', row: any): any;
}
const emit = defineEmits<Emits>();

/** 提交详情 */
const {
  send: handleSubmit,
  form: formData,
  reset: resetFrom,
  updateForm
} = useForm(
  form =>
    Apis.Menu.post_api_menu_submit({
      data: form,
      transform: () => {
        visible.value = false;
        window.$message?.success($t('common.updateSuccess'));
        emit('refresh', form);
      }
    }),
  {
    immediate: false,
    resetAfterSubmiting: true,
    initialForm: {
      id: 0,
      parentId: 0,
      menuType: MenuTypeEnum.菜单,
      iconType: IconTypeEnum.iconify图标,
      status: true,
      keepAlive: false,
      constant: false,
      hideInMenu: false,
      multiTab: false
    } as FormDataType,
    async middleware(_, next) {
      validate().then(async () => {
        await next();
      });
    }
  }
);
/** 获取详情 */
const { send: getInfo } = useRequest(
  id =>
    Apis.Menu.get_api_menu_getinfo({
      params: { id },
      transform: res => {
        updateForm(res.data);
      }
    }),
  { force: true, immediate: false }
);
const title = computed(() => {
  return formData.value.id ? $t('common.edit') : $t('common.add');
});
// 打开
const openForm = async (id?: string, parentId?: number) => {
  visible.value = true;
  if (id) {
    await getInfo(id);
  } else {
    formData.value.parentId = parentId;
  }
};

const closeForm = () => {
  visible.value = false;
  restoreValidation();
  resetFrom();
};
const localIcons = getLocalIcons();
const localIconOptions = localIcons.map<SelectOption>(item => ({
  label: () => (
    <div class="flex-y-center gap-16px">
      <SvgIcon localIcon={item} class="text-icon" />
      <span>{item}</span>
    </div>
  ),
  value: item
}));

function handleCreateQuery() {
  const buttonItem: MenuQuery = {
    id: 0,
    key: '',
    value: '',
    status: true
  };

  return buttonItem;
}

function handleCreateButton() {
  const buttonItem: MenuButton = {
    id: 0,
    code: '',
    desc: '',
    status: true
  };

  return buttonItem;
}

defineExpose({
  openForm
});
</script>

<template>
  <NModal v-model:show="visible" :title="title" preset="card" class="w-800px" @after-leave="closeForm">
    <NScrollbar class="h-480px pr-20px">
      <NForm ref="formRef" :model="formData" :rules="rules" label-placement="left" :label-width="100">
        <NGrid responsive="screen" item-responsive>
          <NFormItemGi span="24 m:12" :label="$t('page.manage.menu.menuType')" path="menuType">
            <NRadioGroup v-model:value="formData.menuType" :disabled="formData.id != 0">
              <NRadio
                v-for="item in getEnumValue(MenuTypeEnum).filter(
                  item => item != MenuTypeEnum.按钮 && item != MenuTypeEnum.参数
                )"
                :key="item"
                :value="item"
                :label="$t(MenuTypeEnum[item])"
              />
            </NRadioGroup>
          </NFormItemGi>
          <NFormItemGi span="24 m:12" :label="$t('page.manage.menu.title')" path="tilte">
            <NInput
              v-model:value="formData.title"
              :placeholder="$t('common.placeholder') + $t('page.manage.menu.title')"
            />
          </NFormItemGi>
          <NFormItemGi span="24 m:12" :label="$t('page.manage.menu.name')" path="name">
            <NInput
              v-model:value="formData.name"
              :placeholder="$t('common.placeholder') + $t('page.manage.menu.name')"
            />
          </NFormItemGi>
          <NFormItemGi span="24 m:12" :label="$t('page.manage.menu.path')" path="path">
            <NInput
              v-model:value="formData.path"
              :placeholder="$t('common.placeholder') + $t('page.manage.menu.path')"
            />
          </NFormItemGi>
          <NFormItemGi
            v-if="formData.menuType === MenuTypeEnum.菜单"
            span="24 m:12"
            :label="$t('page.manage.menu.pathParam')"
            path="pathParam"
          >
            <NInput
              v-model:value="formData.pathParam"
              :placeholder="$t('common.placeholder') + $t('page.manage.menu.pathParam')"
            />
          </NFormItemGi>

          <NFormItemGi
            v-if="formData.parentId == 0 && formData.menuType === MenuTypeEnum.目录"
            span="24 m:12"
            :label="$t('page.manage.menu.layout')"
            path="layout"
          >
            <NSelect
              v-model:value="formData.layout"
              :options="getEnumValue(LayoutTypeEnum).map(item => ({ label: LayoutTypeEnum[item], value: item }))"
              :placeholder="$t('page.manage.menu.form.layout')"
            />
          </NFormItemGi>

          <NFormItemGi
            v-if="formData.menuType === MenuTypeEnum.菜单"
            span="24 m:12"
            :label="$t('page.manage.menu.component')"
            path="page"
          >
            <!--
 <NSelect
              v-model:value="formData.page"
              :options="pageOptions"
              :placeholder="$t('page.manage.menu.form.page')"
            />
-->
            <NInput
              v-model:value="formData.component"
              :placeholder="$t('common.placeholder') + $t('page.manage.menu.component')"
            />
          </NFormItemGi>
          <NFormItemGi span="24 m:12" :label="$t('page.manage.menu.i18nKey')" path="i18nKey">
            <NInput
              v-model:value="formData.i18nKey"
              :placeholder="$t('common.placeholder') + $t('page.manage.menu.i18nKey')"
            />
          </NFormItemGi>
          <NFormItemGi span="24 m:12" :label="$t('page.manage.menu.order')" path="order">
            <NInputNumber
              v-model:value="formData.order"
              class="w-full"
              :placeholder="$t('common.placeholder') + $t('page.manage.menu.order')"
            />
          </NFormItemGi>
          <NFormItemGi span="24 m:12" :label="$t('page.manage.menu.iconTypeTitle')" path="iconType">
            <NRadioGroup v-model:value="formData.iconType">
              <NRadio
                v-for="item in getEnumValue(IconTypeEnum)"
                :key="item"
                :value="item"
                :label="$t(IconTypeEnum[item])"
              />
            </NRadioGroup>
          </NFormItemGi>
          <NFormItemGi span="24 m:12" :label="$t('page.manage.menu.icon')" path="icon">
            <template v-if="formData.iconType === IconTypeEnum.iconify图标">
              <NInput
                v-model:value="formData.icon"
                :placeholder="$t('common.placeholder') + $t('page.manage.menu.form.icon')"
                class="flex-1"
              >
                <template #suffix>
                  <SvgIcon v-if="formData.icon" :icon="formData.icon" class="text-icon" />
                </template>
              </NInput>
            </template>
            <template v-if="formData.iconType === IconTypeEnum.本地图标">
              <NSelect
                v-model:value="formData.icon"
                :placeholder="$t('page.manage.menu.form.localIcon')"
                :options="localIconOptions"
              />
            </template>
          </NFormItemGi>
          <NFormItemGi span="24 m:12" :label="$t('page.manage.menu.menuStatus')" path="status">
            <NRadioGroup v-model:value="formData.status">
              <NRadio :value="true" label="启用" />
              <NRadio :value="false" label="禁用" />
            </NRadioGroup>
          </NFormItemGi>
          <NFormItemGi span="24 m:12" :label="$t('page.manage.menu.keepAlive')" path="keepAlive">
            <NRadioGroup v-model:value="formData.keepAlive">
              <NRadio :value="true" :label="$t('common.yesOrNo.yes')" />
              <NRadio :value="false" :label="$t('common.yesOrNo.no')" />
            </NRadioGroup>
          </NFormItemGi>
          <NFormItemGi span="24 m:12" :label="$t('page.manage.menu.constant')" path="constant">
            <NRadioGroup v-model:value="formData.constant">
              <NRadio :value="true" :label="$t('common.yesOrNo.yes')" />
              <NRadio :value="false" :label="$t('common.yesOrNo.no')" />
            </NRadioGroup>
          </NFormItemGi>
          <NFormItemGi span="24 m:12" :label="$t('page.manage.menu.href')" path="href">
            <NInput
              v-model:value="formData.href"
              :placeholder="$t('common.placeholder') + $t('page.manage.menu.form.href')"
            />
          </NFormItemGi>
          <NFormItemGi span="24 m:12" :label="$t('page.manage.menu.hideInMenu')" path="hideInMenu">
            <NRadioGroup v-model:value="formData.hideInMenu">
              <NRadio :value="true" :label="$t('common.yesOrNo.yes')" />
              <NRadio :value="false" :label="$t('common.yesOrNo.no')" />
            </NRadioGroup>
          </NFormItemGi>
          <NFormItemGi
            v-if="formData.hideInMenu"
            span="24 m:12"
            :label="$t('page.manage.menu.activeMenu')"
            path="activeMenu"
          >
            <!--
 <NSelect
              v-model:value="formData.activeMenu"
              :options="pageOptions"
              clearable
              :placeholder="$t('page.manage.menu.form.activeMenu')"
            />
-->
          </NFormItemGi>
          <NFormItemGi span="24 m:12" :label="$t('page.manage.menu.multiTab')" path="multiTab">
            <NRadioGroup v-model:value="formData.multiTab">
              <NRadio :value="true" :label="$t('common.yesOrNo.yes')" />
              <NRadio :value="false" :label="$t('common.yesOrNo.no')" />
            </NRadioGroup>
          </NFormItemGi>
          <NFormItemGi span="24 m:12" :label="$t('page.manage.menu.fixedIndexInTab')" path="fixedIndexInTab">
            <NInputNumber
              v-model:value="formData.fixedIndexInTab"
              class="w-full"
              clearable
              :placeholder="$t('common.placeholder') + $t('page.manage.menu.form.fixedIndexInTab')"
            />
          </NFormItemGi>

          <NFormItemGi span="24" :label="$t('page.manage.menu.query')">
            <NDynamicInput v-model:value="formData.querys" preset="pair" :on-create="handleCreateQuery">
              <template #default="{ value }">
                <div class="ml-8px flex-y-center flex-1 gap-12px">
                  <NInput
                    v-model:value="value.key"
                    :placeholder="$t('common.placeholder') + $t('page.manage.menu.form.queryKey')"
                    class="flex-1"
                  />
                  <NInput
                    v-model:value="value.value"
                    :placeholder="$t('common.placeholder') + $t('page.manage.menu.form.queryValue')"
                    class="flex-1"
                  />
                </div>
              </template>
              <template #action="{ index, create, remove }">
                <NSpace class="ml-12px">
                  <NButton size="medium" @click="() => create(index)">
                    <icon-ic:round-plus class="text-icon" />
                  </NButton>
                  <NButton size="medium" @click="() => remove(index)">
                    <icon-ic-round-remove class="text-icon" />
                  </NButton>
                </NSpace>
              </template>
            </NDynamicInput>
          </NFormItemGi>

          <NFormItemGi span="24" :label="$t('page.manage.menu.button')">
            <NDynamicInput v-model:value="formData.buttons" :on-create="handleCreateButton">
              <template #default="{ value }">
                <div class="ml-8px flex-y-center flex-1 gap-12px">
                  <NInput
                    v-model:value="value.code"
                    :placeholder="$t('common.placeholder') + $t('page.manage.menu.form.buttonCode')"
                    class="flex-1"
                  />
                  <NInput
                    v-model:value="value.desc"
                    :placeholder="$t('common.placeholder') + $t('page.manage.menu.form.buttonDesc')"
                    class="flex-1"
                  />
                </div>
              </template>
              <template #action="{ index, create, remove }">
                <NSpace class="ml-12px">
                  <NButton size="medium" @click="() => create(index)">
                    <icon-ic:round-plus class="text-icon" />
                  </NButton>
                  <NButton size="medium" @click="() => remove(index)">
                    <icon-ic-round-remove class="text-icon" />
                  </NButton>
                </NSpace>
              </template>
            </NDynamicInput>
          </NFormItemGi>
        </NGrid>
      </NForm>
    </NScrollbar>
    <template #footer>
      <NSpace justify="end" :size="16">
        <NButton @click="closeForm">{{ $t('common.cancel') }}</NButton>
        <NButton type="primary" @click="handleSubmit">{{ $t('common.confirm') }}</NButton>
      </NSpace>
    </template>
  </NModal>
</template>
