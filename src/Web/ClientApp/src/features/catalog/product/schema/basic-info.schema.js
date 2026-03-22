import z from "zod";

export const basicInfoSchema = z.object({
  name: z.string().trim().nonempty("Product name is required"),
  description: z.string().trim().nonempty("Product description is required"),
  category: z
    .string()
    .trim()
    .nonempty("Product category is required")
    .maxLength(5000, "Category must be less than 5000 characters"),
});
