<script setup lang="tsx">
import { computed, h, ref } from 'vue';
import { useForm, useRequest } from 'alova/client';
import type { TreeOption } from 'naive-ui';
import { NButton, NTag } from 'naive-ui';
import { $t } from '@/locales';
import type { SetByRoleIdDTO } from '@/api/globals';
import { MenuTypeEnum } from '@/api/apiEnums';
import { MenuTypeEl } from '@/api/apiEls';
defineOptions({
  name: 'MenuTree'
});
const visible = ref<boolean>(false);

interface Emits {
  (e: 'refresh'): void;
}
const emit = defineEmits<Emits>();

/** 获取数据Tree */
const { data } = useRequest(
  () =>
    Apis.Menu.get_api_menu_select({
      transform: res => {
        return res.data;
      }
    }),
  {
    immediate: true
  }
);

/** 提交详情 */
const {
  send: handleSubmit,
  form: formData,
  reset: resetFrom,
  updateForm
} = useForm(
  form =>
    Apis.Menu.post_api_menu_setbyroleid({
      data: form,
      transform: () => {
        visible.value = false;
        resetFrom();
        window.$message?.success($t('common.updateSuccess'));
        emit('refresh');
      }
    }),
  {
    immediate: false,
    resetAfterSubmiting: true,
    initialForm: {} as SetByRoleIdDTO
  }
);
/** 获取数据getbyroleid */
const { send: getData, data: treeIds } = useRequest(
  roleId =>
    Apis.Menu.get_api_menu_getbyroleid({
      params: { roleId },
      transform: res => {
        updateForm({ roleId, menuIds: res.data });
        return res.data;
      }
    }),
  {
    force: true,
    immediate: false
  }
);
const title = computed(() => {
  return '角色权限';
});
// 打开
const openForm = async (id?: string) => {
  resetFrom();
  visible.value = true;
  await getData(id);
};

const closeForm = () => {
  resetFrom();
};
const renderSuffix = ({ option }: { option: TreeOption }) => {
  return h(NTag, { type: MenuTypeEl[option.menuType as number] }, MenuTypeEnum[option.menuType as number]);
};

defineExpose({
  openForm
});
</script>

<template>
  <NModal v-model:show="visible" :title="title" preset="card" class="w-480px">
    <!--     :loading="loading && dataLoading" -->
    <NTree
      v-model:checked-keys="formData.menuIds"
      :default-expanded-keys="treeIds"
      :data="data"
      label-field="title"
      key-field="id"
      checkable
      expand-on-click
      virtual-scroll
      block-line
      class="h-680px"
      :render-suffix="renderSuffix"
    />
    <template #footer>
      <NSpace justify="end">
        <NButton size="small" class="mt-16px" @click="closeForm">
          {{ $t('common.cancel') }}
        </NButton>
        <NButton type="primary" size="small" class="mt-16px" @click="handleSubmit">
          {{ $t('common.confirm') }}
        </NButton>
      </NSpace>
    </template>
  </NModal>
</template>

<style scoped></style>
