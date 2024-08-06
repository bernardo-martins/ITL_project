using Bitmovin.Api.Sdk.Models;
using Stream = Bitmovin.Api.Sdk.Models.Stream;

namespace Anikatze.Application.Services;
using Bitmovin.Api.Sdk;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bitmovin.Api.Sdk.Common;
using Bitmovin.Api.Sdk.Common.Logging;
using Microsoft.Extensions.Logging;
public class BitmovinService
{
    private readonly BitmovinApi _bitmovinApi;
    private readonly ILogger<BitmovinService> _logger;
    private AzureInput _azureInput;
    private AzureOutput _azureOutput;
    private Bitmovin.Api.Sdk.Models.Encoding _encoding;
    private string _inputId;
    private string _outputId;
    private string _videoCodecConfigId1;
    private string _videoCodecConfigId2;
    private string _videoCodecConfigId3;
    private Stream videoStream1;
    private Stream videoStream2;
    private Stream videoStream3;
    private EncodingOutput videoMuxingOutput1;
    private EncodingOutput videoMuxingOutput2;
    private EncodingOutput videoMuxingOutput3;
    private HlsManifestDefault hlsManifest;
    private string baseOutputPath = "output/";

    public BitmovinService(ILogger<BitmovinService> logger)
    {
        _bitmovinApi = BitmovinApi.Builder
            .WithApiKey("0d078859-6427-4cfb-a503-f56017175551")
            .Build();
        _logger = logger;
    }

    public async Task<AzureInput> CreateAzureInput()
    {
        try
        {
            _azureInput = await _bitmovinApi.Encoding.Inputs.Azure.CreateAsync(new AzureInput
            {
                Name = "Azure_Blob_Input",
                Description = "Azure Blob Storage Input MOV",
                AccountName = "anikatzetest",
                AccountKey = "/Po134FhTBLqmVTX3cr8UT9v+6AopOOwoXxJPFlEcBcJp6z1cHMX6I+bI1vxkgApaPjWtDxzwC5N+AStvFAMKw==",
                Container = "bitmovin"
            });
            _inputId = _azureInput.Id;
            _logger.LogInformation($"Azure input created with ID: {_inputId}");
            return _azureInput;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating Azure input: {ex.Message}");
            throw;
        }
    }

    public async Task<AzureOutput> CreateAzureOutput()
    {
        try
        {
            _azureOutput = await _bitmovinApi.Encoding.Outputs.Azure.CreateAsync(new AzureOutput
            {
                Name = "Azure_Blob_Output",
                Description = "Azure Blob Storage Output",
                AccountName = "anikatzetest",
                AccountKey = "/Po134FhTBLqmVTX3cr8UT9v+6AopOOwoXxJPFlEcBcJp6z1cHMX6I+bI1vxkgApaPjWtDxzwC5N+AStvFAMKw==",
                Container = "bitmovin"
            });
            _outputId = _azureOutput.Id;
            _logger.LogInformation($"Azure output created with ID: {_outputId}");
            return _azureOutput;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating Azure output: {ex.Message}");
            throw;
        }
    }

    public async Task CreateEncodingHLS()
    {
        try
        {
            var codecConfigVideoName1 = "Getting Started H264 Codec Config 1";
            var codecConfigVideoBitrate1 = 1500000;
            var videoCodecConfiguration1 = await _bitmovinApi.Encoding.Configurations.Video.H264.CreateAsync(new H264VideoConfiguration
            {
                Name = codecConfigVideoName1,
                PresetConfiguration = PresetConfiguration.VOD_STANDARD,
                Width = 1024,
                Bitrate = codecConfigVideoBitrate1,
                Description = codecConfigVideoName1 + "_" + codecConfigVideoBitrate1
            });
            _videoCodecConfigId1 = videoCodecConfiguration1.Id;

            var codecConfigVideoName2 = "Getting Started H264 Codec Config 2";
            var codecConfigVideoBitrate2 = 1000000;
            var videoCodecConfiguration2 = await _bitmovinApi.Encoding.Configurations.Video.H264.CreateAsync(new H264VideoConfiguration
            {
                Name = codecConfigVideoName2,
                PresetConfiguration = PresetConfiguration.VOD_STANDARD,
                Width = 768,
                Bitrate = codecConfigVideoBitrate2,
                Description = codecConfigVideoName2 + "_" + codecConfigVideoBitrate2
            });
            _videoCodecConfigId2 = videoCodecConfiguration2.Id;

            var codecConfigVideoName3 = "H264 Codec Config 3";
            var codecConfigVideoBitrate3 = 750000;
            var videoCodecConfiguration3 = await _bitmovinApi.Encoding.Configurations.Video.H264.CreateAsync(new H264VideoConfiguration
            {
                Name = codecConfigVideoName3,
                PresetConfiguration = PresetConfiguration.VOD_STANDARD,
                Width = 640,
                Bitrate = codecConfigVideoBitrate3,
                Description = codecConfigVideoName3 + "_" + codecConfigVideoBitrate3
            });
            _videoCodecConfigId3 = videoCodecConfiguration3.Id;
            _logger.LogInformation("HLS configurations created successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating HLS configurations: {ex.Message}");
            throw;
        }
    }

    public async Task CreateEncoding()
    {
        try
        {
            _encoding = await _bitmovinApi.Encoding.Encodings.CreateAsync(new Bitmovin.Api.Sdk.Models.Encoding
            {
                Name = "Getting Started Encoding",
                CloudRegion = CloudRegion.GOOGLE_EUROPE_WEST_1
            });
            _logger.LogInformation($"Encoding created with ID: {_encoding.Id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating encoding: {ex.Message}");
            throw;
        }
    }

    public async Task CreateStreams()
    {
        string inputPath = "AnikatzeVideoBitmovin.mov";
        try
        {
            var videoStreamInput = new StreamInput
            {
                InputId = _inputId,
                InputPath = inputPath,
                SelectionMode = StreamSelectionMode.AUTO
            };

            videoStream1 = await _bitmovinApi.Encoding.Encodings.Streams.CreateAsync(_encoding.Id, new Stream
            {
                InputStreams = new List<StreamInput>
                {
                    videoStreamInput
                },
                CodecConfigId = _videoCodecConfigId1
            });

            videoStream2 = await _bitmovinApi.Encoding.Encodings.Streams.CreateAsync(_encoding.Id, new Stream
            {
                InputStreams = new List<StreamInput>
                {
                    videoStreamInput
                },
                CodecConfigId = _videoCodecConfigId2
            });

            videoStream3 = await _bitmovinApi.Encoding.Encodings.Streams.CreateAsync(_encoding.Id, new Stream
            {
                InputStreams = new List<StreamInput>
                {
                    videoStreamInput
                },
                CodecConfigId = _videoCodecConfigId3
            });
            _logger.LogInformation("Streams created successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating streams: {ex.Message}");
            throw;
        }
    }

    public async Task CreateMuxings()
    {
        var aclEntry = new AclEntry { Permission = AclPermission.PUBLIC_READ };
        double segmentLength = 4;
        string segmentNaming = "seg_%number%.ts";

        try
        {
            videoMuxingOutput1 = new EncodingOutput
            {
                Acl = new List<AclEntry> { aclEntry },
                OutputId = _outputId,
                OutputPath = baseOutputPath + "video/1024_1500000/ts"
            };

            var videoMuxing1 = new TsMuxing
            {
                Outputs = new List<EncodingOutput> { videoMuxingOutput1 },
                Streams = new List<MuxingStream> { new MuxingStream { StreamId = videoStream1.Id } },
                SegmentLength = segmentLength,
                SegmentNaming = segmentNaming
            };
            videoMuxing1 = await _bitmovinApi.Encoding.Encodings.Muxings.Ts.CreateAsync(_encoding.Id, videoMuxing1);

            videoMuxingOutput2 = new EncodingOutput
            {
                Acl = new List<AclEntry> { aclEntry },
                OutputId = _outputId,
                OutputPath = baseOutputPath + "video/768_1000000/ts"
            };

            var videoMuxing2 = new TsMuxing
            {
                Outputs = new List<EncodingOutput> { videoMuxingOutput2 },
                Streams = new List<MuxingStream> { new MuxingStream { StreamId = videoStream2.Id } },
                SegmentLength = segmentLength,
                SegmentNaming = segmentNaming
            };
            videoMuxing2 = await _bitmovinApi.Encoding.Encodings.Muxings.Ts.CreateAsync(_encoding.Id, videoMuxing2);

            videoMuxingOutput3 = new EncodingOutput
            {
                Acl = new List<AclEntry> { aclEntry },
                OutputId = _outputId,
                OutputPath = baseOutputPath + "video/640_750000/ts"
            };

            var videoMuxing3 = new TsMuxing
            {
                Outputs = new List<EncodingOutput> { videoMuxingOutput3 },
                Streams = new List<MuxingStream> { new MuxingStream { StreamId = videoStream3.Id } },
                SegmentLength = segmentLength,
                SegmentNaming = segmentNaming
            };
            videoMuxing3 = await _bitmovinApi.Encoding.Encodings.Muxings.Ts.CreateAsync(_encoding.Id, videoMuxing3);
            _logger.LogInformation("Muxings created successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating muxings: {ex.Message}");
            throw;
        }
    }

    public async Task CreateHlsManifest()
    {
        try
        {
            hlsManifest = new HlsManifestDefault
            {
                EncodingId = _encoding.Id,
                Name = "stream.m3u8",
                Version = HlsManifestDefaultVersion.V1,
                Outputs = new List<EncodingOutput>
                {
                    new EncodingOutput(){
                        OutputId = _outputId,
                        OutputPath = baseOutputPath,  // Set the manifest output path to the base output path
                        Acl = new List<AclEntry> { new AclEntry { Permission = AclPermission.PUBLIC_READ } }
                    }
                }
            };

            hlsManifest = await _bitmovinApi.Encoding.Manifests.Hls.Default.CreateAsync(hlsManifest);
            _logger.LogInformation("HLS manifest created successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating HLS manifest: {ex.Message}");
            throw;
        }
    }

    public async Task StartEncoding()
    {
        try
        {
            var startEncodingRequest = new StartEncodingRequest
            {
                ManifestGenerator = ManifestGenerator.V2,
                VodHlsManifests = new List<ManifestResource>
                {
                    new ManifestResource
                    {
                        ManifestId = hlsManifest.Id
                    }
                }
            };
            await _bitmovinApi.Encoding.Encodings.StartAsync(_encoding.Id, startEncodingRequest);
            _logger.LogInformation("Encoding started successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error starting encoding: {ex.Message}");
            throw;
        }
    }
}
