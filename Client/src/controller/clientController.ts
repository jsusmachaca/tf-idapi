// process.loadEnvFile() // uncomment in development mode
import { Request, Response } from "express"
import axios from 'axios'

const API_SERVER = process.env.API_SERVER!

export const getAll = async (_req: Request, res: Response) => {
  try {
    const reqServer: any  = await axios.get(`http://${API_SERVER}/api/pelicula`)

    return res.render('index', { data: reqServer.data })
  } catch (err) {
    console.log(err)
    return res.status(500).json({ error: "an error ocurred" })
  }
}

export const searchTF = async (req: Request, res: Response) => {
  try {
    const { text } = req.params
    const reqServer: any  = await axios.get(`http://${API_SERVER}/api/pelicula/search/${text}`)

    return res.render('index', { data: reqServer.data })
  } catch (err) {
    console.log(err)
    return res.status(500).json({ error: "an error ocurred" })
  }
}
