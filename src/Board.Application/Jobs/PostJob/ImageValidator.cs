using FluentValidation;

namespace Board.Application.Jobs.PostJob
{
    public static class ImageValidatorExtensions
    {
        private static byte[] PngHeader = { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };

        public static IRuleBuilderInitial<T, byte[]> IsPngImage<T>(this IRuleBuilder<T, byte[]> ruleBuilder, int maxSize)
        {
            return ruleBuilder.Custom((data, context) =>
            {
                if (data == null) return;

                if (data.Length < PngHeader.Length)
                {
                    context.AddFailure($"Not a valid png file.");
                    return;
                }

                for (int i = 0; i < PngHeader.Length; i++)
                {
                    if (data[i] != PngHeader[i])
                    {
                        context.AddFailure($"Not a valid png file.");
                        return;
                    }
                }

                if (data.Length > maxSize)
                {
                    context.AddFailure($"Png size ({data.Length}) is greater than allowed ({maxSize}).");
                }

            });
        }
    }
}
