<template>
  <div>
    <el-card header="Josn Redis拦截器 AOP , 建议大家合理使用,  建议数据库访问层(DAL)使用,确保数据源没被污染">
      <el-pagination
        :page-sizes="[100, 200, 500, 1000]"
        layout="total, sizes, prev, pager, next, jumper"
        :total="total"
        v-model:current-page="page"
        v-model:page-size="pageSize"
      /><span style="color: red">
        这里使用分页接口, 第一次请求走的 数据后面都走的 [RedisInterceptor] 注解,
        详情请看源码,这只是做演示看请求速率,(请求数据请看 数据库测压) 只存了30秒
      </span>
      <div v-for="t in times" :key="t">{{ t }}</div>
    </el-card>
  </div>
</template>

<script setup lang="ts">
const sortList = ref<Record<string, string>>({ id: "asc" });
const times = ref<string[]>([]);
/**
 * 获取数据
 */
const {
  data,
  page,
  pageSize,
  total,
  loading,
  send: getData,
  reload,
} = usePagination(
  // Method实例获取函数，它将接收page和pageSize，并返回一个Method实例
  (page, pageSize) =>
    Apis.Demo.get_api_demo_getpage({
      params: {
        PageIndex: page,
        pageSize: pageSize,
        sortList: sortList.value,
      },
      transform: (res) => {
        times.value.push(res.msg || "");
        return res;
      },
    }),
  {
    force: true,
    watchingStates: [sortList],
    initialPage: 1, // 初始页码，默认为1
    initialPageSize: 10, // 初始每页数据条数，默认为10
    preloadPreviousPage: false, // 是否预加载下一页
    preloadNextPage: false, // 是否预加载上一页
    total: ({ data }) => data?.pagerInfo?.totalRowCount,
    data: ({ data }) => data?.data,
  }
);
</script>
