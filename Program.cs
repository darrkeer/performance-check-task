// Maybe check if the game is downloaded

var utils = Utils.getPlatformUtils();

// prepare output file
{
    using var writer = new StreamWriter(utils.getOutputFilePath());
    writer.Write("");
}

Benchmark cpuBench = new Benchmark("cpu", utils.getCPUConfigFilePath());
cpuBench.Start(utils);

Benchmark gpuBench = new Benchmark("gpu", utils.getGPUConfigFilePath());
gpuBench.Start(utils);

SpecsGrabber grabber = SpecsGrabber.getPlatformSpecsGrabber();
grabber.Write(utils);
