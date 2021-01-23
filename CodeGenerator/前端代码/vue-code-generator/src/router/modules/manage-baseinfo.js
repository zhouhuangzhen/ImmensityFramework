//  基础信息管理

import Layout from '@/layout'

const componentsRouter = {
  path: '/baseinfos',
  component: Layout,
  redirect: 'noRedirect',
  name: 'BaseInfo',
  meta: {
    title: 'baseinfo',
    icon: 'component'
  },
  children: [
    {
      path: 'parameterinfo',
      component: () => import('@/views/manage-baseinfo/parameter-info'),
      name: 'ParameterInfo',
      meta: { title: 'parameterinfo' }
    }
  ]
}

export default componentsRouter
