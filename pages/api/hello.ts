// Next.js API route support: https://nextjs.org/docs/api-routes/introduction

import { NextApiRequest, NextApiResponse } from "next"

interface Res {
  name: string
}

export default (req: NextApiRequest, res: NextApiResponse<Res>) => {
  res.status(200).json({ name: 'John Doe' })
}
